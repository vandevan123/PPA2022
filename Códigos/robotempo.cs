string cor(string sensor) {
	return Bot.GetComponent<ColorSensor>(sensor).Analog.ToString();
}
double distancia(string sensorsonico){
    return Bot.GetComponent<UltrasonicSensor>(sensorsonico).Analog;
}
double angulo(double antigo){
    double novo = Bot.Compass;
    if (novo-antigo>270) {
        return 360-novo+antigo;
    }
    else if (antigo-novo>270) {
        return 360-antigo+novo;
    }
    else if (novo>=antigo){
        return novo-antigo; 
    }
    else{
        return antigo-novo; 
    }
     
}
void motor(string nome, int forca, int vel) {
	Bot.GetComponent<Servomotor>(nome).Locked = false;
	Bot.GetComponent<Servomotor>(nome).Apply(forca, vel);
}
async Task subirEscavadeira(string nome, int forca, int vel) {
	motor(nome, forca, vel);
	while(Math.Abs(Bot.GetComponent<Servomotor>(nome).Angle)  > 135) {
	 	await Time.Delay(100);
	}
	motor(nome, 1000, 0);
    Bot.GetComponent<Servomotor>(nome).Locked = true;
}
async Task descerEscavedeira(string nome, int forca, int vel) {
	motor(nome, forca, -vel);
	while(Math.Abs(Bot.GetComponent<Servomotor>(nome).Angle)  < 170) {
		await Time.Delay(100);
	}
	motor(nome, 1000, 0);
    Bot.GetComponent<Servomotor>(nome).Locked = true;
}

async Task Main() {
	IO.OpenConsole();
	IO.ClearPrint();
	int veloFrente = 300;
    int veloCurva = 200;
    int veloSubida = 1000;
    int veloSubidaCurva = 200;
    int veloDescida = 100;
    int veloDescidaCurva = 50;

	string estado = "indo pra frente";
    string estadoNaTela = "";

	while(true) {
		if (estadoNaTela!=estado) {
			IO.PrintLine(estado);
			estadoNaTela = estado;
		}

        if (estado == "indo pra frente"){
            IO.PrintLine(distancia("ultra").ToString());
            motor("flmotor", 100, veloFrente);
            motor("frmotor", 100, veloFrente);
            motor("blmotor", 100, veloFrente);
            motor("brmotor", 100, veloFrente);
            if ((cor("lc1")=="Preto" || cor("lc2")=="Preto")) {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "fazendo curva esquerda";
            }
            if ((cor("rc1")=="Preto" || cor("rc2")=="Preto")) {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "fazendo curva direita";
            }
            if (Bot.Inclination>=330 && Bot.Inclination<=350) {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "subindo rampa frente";
            }
            if (Bot.Inclination>=10 && Bot.Inclination<=30) {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "descendo rampa frente";
            }
            if(distancia("ultra") <= 5 && distancia("ultra") > 0){
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "objeto a frente";
            }
        }
        else if(estado == "objeto a frente"){
            motor("flmotor", 100, 3*veloCurva);
            motor("frmotor", 100, -veloCurva);
            motor("blmotor", 100, 3*veloCurva);
            motor("brmotor", 100, -veloCurva);
            await Time.Delay(1500);
            motor("flmotor", 1000, 0);
            motor("frmotor", 1000, 0);
            motor("blmotor", 1000, 0);
            motor("brmotor", 1000, 0);
            estado = "preparar para desvio";
            await Time.Delay(600);
            motor("flmotor", 100, veloFrente);
            motor("frmotor", 100, veloFrente);
            motor("blmotor", 100, veloFrente);
            motor("brmotor", 100, veloFrente);
            await Time.Delay(800); // tempo de espera
            motor("flmotor", 1000, 0);
            motor("frmotor", 1000, 0);
            motor("blmotor", 1000, 0);
            motor("brmotor", 1000, 0);
            estado = "giro 90 graus para esquerda";
            await Time.Delay(3000);
            motor("flmotor", 100, -veloCurva);
            motor("frmotor", 100, 3*veloCurva);
            motor("blmotor", 100, -veloCurva);
            motor("brmotor", 100, 3*veloCurva);
            await Time.Delay(600); // tempo de espera
            motor("flmotor", 1000, 0);
            motor("frmotor", 1000, 0);
            motor("blmotor", 1000, 0);
            motor("brmotor", 1000, 0);
        }
        
        
        else if (estado == "fazendo curva esquerda") {
            motor("flmotor", 100, -veloCurva);
            motor("frmotor", 100, 2*veloCurva);
            motor("blmotor", 100, -veloCurva);
            motor("brmotor", 100, 2*veloCurva);
            if (cor("cc")=="Preto" && cor("lc1")=="Branco" && cor("rc1")=="Branco") {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "indo pra frente";
            }
        }
        else if (estado == "fazendo curva direita") {
            motor("flmotor", 100, 2*veloCurva);
            motor("frmotor", 100, -veloCurva);
            motor("blmotor", 100, 2*veloCurva);
            motor("brmotor", 100, -veloCurva);
            if (cor("cc")=="Preto" && cor("lc1")=="Branco" && cor("rc1")=="Branco") {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "indo pra frente";
            }
        }
        else if (estado == "subindo rampa frente") {
            motor("flmotor", 5000, veloSubida);
            motor("frmotor", 5000, veloSubida);
            motor("blmotor", 5000, veloSubida);
            motor("brmotor", 5000, veloSubida);
            if ((cor("lc1")=="Preto" || cor("lc2")=="Preto")) {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "subindo rampa esquerda";
            }
            if ((cor("rc1")=="Preto" || cor("rc2")=="Preto")) {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "subindo rampa direita";
            }
            if (Bot.Inclination<10 || Bot.Inclination>350) {
                await Time.Delay(500);
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "indo pra frente";
            }
            if (cor("lc2")=="Ciano" || cor("lc1")=="Ciano" || cor("cc")=="Ciano" || cor("rc1")=="Ciano" || cor("rc2")=="Ciano") {
                motor("flmotor", 5000, veloSubida);
                motor("frmotor", 5000, veloSubida);
                motor("blmotor", 5000, veloSubida);
                motor("brmotor", 5000, veloSubida);
                estado = "finalizando subida";
            }
            if (cor("lc2")=="Preto" && cor("lc1")=="Preto" && cor("cc")=="Preto" && cor("rc1")=="Preto" && cor("rc2")=="Preto") {
                motor("flmotor", 5000, veloSubida);
                motor("frmotor", 5000, veloSubida);
                motor("blmotor", 5000, veloSubida);
                motor("brmotor", 5000, veloSubida);
                estado = "finalizando subida";
            }
        }
        else if (estado == "subindo rampa esquerda") {
            motor("flmotor", 1000, -veloSubidaCurva);
            motor("frmotor", 1000, 2*veloSubidaCurva);
            motor("blmotor", 1000, -veloSubidaCurva);
            motor("brmotor", 1000, 2*veloSubidaCurva);
            if (cor("cc")=="Preto" && cor("lc1")=="Branco" && cor("rc1")=="Branco") {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "subindo rampa frente";
            }
            if (cor("lc2")=="Ciano" || cor("lc1")=="Ciano" || cor("cc")=="Ciano" || cor("rc1")=="Ciano" || cor("rc2")=="Ciano") {
                motor("flmotor", 5000, veloSubida);
                motor("frmotor", 5000, veloSubida);
                motor("blmotor", 5000, veloSubida);
                motor("brmotor", 5000, veloSubida);
                estado = "finalizando subida";
            }
            if (cor("lc2")=="Preto" && cor("lc1")=="Preto" && cor("cc")=="Preto" && cor("rc1")=="Preto" && cor("rc2")=="Preto") {
                motor("flmotor", 5000, veloSubida);
                motor("frmotor", 5000, veloSubida);
                motor("blmotor", 5000, veloSubida);
                motor("brmotor", 5000, veloSubida);
                estado = "finalizando subida";
            }
        }
        else if (estado == "subindo rampa direita") {
            motor("flmotor", 1000, 2*veloSubidaCurva);
            motor("frmotor", 1000, -veloSubidaCurva);
            motor("blmotor", 1000, 2*veloSubidaCurva);
            motor("brmotor", 1000, -veloSubidaCurva);
            if (cor("cc")=="Preto" && cor("lc1")=="Branco" && cor("rc1")=="Branco") {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "subindo rampa frente";
            }
            if (cor("lc2")=="Ciano" || cor("lc1")=="Ciano" || cor("cc")=="Ciano" || cor("rc1")=="Ciano" || cor("rc2")=="Ciano") {
                motor("flmotor", 5000, veloSubida);
                motor("frmotor", 5000, veloSubida);
                motor("blmotor", 5000, veloSubida);
                motor("brmotor", 5000, veloSubida);
                estado = "finalizando subida";
            }
            if (cor("lc2")=="Preto" && cor("lc1")=="Preto" && cor("cc")=="Preto" && cor("rc1")=="Preto" && cor("rc2")=="Preto") {
                motor("flmotor", 5000, veloSubida);
                motor("frmotor", 5000, veloSubida);
                motor("blmotor", 5000, veloSubida);
                motor("brmotor", 5000, veloSubida);
                estado = "finalizando subida";
            }
        }
        else if (estado == "descendo rampa frente") {
            motor("flmotor", 300, veloDescida);
            motor("frmotor", 300, veloDescida);
            motor("blmotor", 300, veloDescida);
            motor("brmotor", 300, veloDescida);
            if ((cor("lc1")=="Preto" || cor("lc2")=="Preto")) {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "descendo rampa esquerda";
            }
            if ((cor("rc1")=="Preto" || cor("rc2")=="Preto")) {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "descendo rampa direita";
            }
            if (Bot.Inclination<10 || Bot.Inclination>350) {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "indo pra frente";
            }
        }
        else if (estado == "descendo rampa esquerda") {
            motor("flmotor", 1000, -veloDescidaCurva);
            motor("frmotor", 1000, 2*veloDescidaCurva);
            motor("blmotor", 1000, -veloDescidaCurva);
            motor("brmotor", 1000, 2*veloDescidaCurva);
            if (cor("cc")=="Preto" && cor("lc1")=="Branco" && cor("rc1")=="Branco") {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "descendo rampa frente";
            }
        }
        else if (estado == "descendo rampa direita") {
            motor("flmotor", 1000, 2*veloDescidaCurva);
            motor("frmotor", 1000, -veloDescidaCurva);
            motor("blmotor", 1000, 2*veloDescidaCurva);
            motor("brmotor", 1000, -veloDescidaCurva);
            if (cor("cc")=="Preto" && cor("lc1")=="Branco" && cor("rc1")=="Branco") {
                motor("flmotor", 1000, 0);
                motor("frmotor", 1000, 0);
                motor("blmotor", 1000, 0);
                motor("brmotor", 1000, 0);
                estado = "descendo rampa frente";
            }
        }
        else if (estado == "finalizando subida") {
            if (Bot.Inclination>350 || Bot.Inclination<10) {
                await Time.Delay(2000);
                motor("flmotor", 5000, 0);
                motor("frmotor", 5000, 0);
                motor("blmotor", 5000, 0);
                motor("brmotor", 5000, 0);
                estado = "fim do seguidor de linha";
            }
        }

        await Time.Delay(40);
	}	
}