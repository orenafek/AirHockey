/*
 Name:		TableControl.ino
 Created:	12/4/2015 7:14:41 PM
 Author:	Oren
*/

const int LED = 13;
const unsigned long BAND = 9600;
const int TRESHHOLD = 70;
int photoValue = 0;

String transmition;

int result;

// the setup function runs once when you press reset or power the board
void setup() {
	pinMode(13, OUTPUT);
	Serial.begin(BAND);
}

// the loop function runs over and over again until power down or reset
void loop() {

	transmition = "";

	//TODO: FOR MEGA CHANGE TO :
	// while(Serial.available() > 0){
	while (Serial.available() > 0) {
		transmition += char(Serial.read());
	}

	if (transmition == "") {
		return;
	}

	if(transmition.equalsIgnoreCase("ON")){
		digitalWrite(LED, HIGH);
	}

	if (transmition.equalsIgnoreCase("OFF")) {
		digitalWrite(LED, LOW);
	}

}
