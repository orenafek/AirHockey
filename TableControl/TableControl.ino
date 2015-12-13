/*
 Name:		TableControl.ino
 Created:	12/4/2015 7:14:41 PM
 Author:	Oren
*/

#include <SoftwareSerial\SoftwareSerial.h>

const int LED = 13;
const unsigned long BAND = 9600;
const int TRESHHOLD = 70;
int photoValue = 0;

String transmition;
SoftwareSerial BT(10, 11);

int trans;
int result;

// the setup function runs once when you press reset or power the board
void setup() {
	pinMode(LED, OUTPUT);
	digitalWrite(LED, LOW);
	BT.begin(9600);

}

// the loop function runs over and over again until power down or reset
void loop() {

	if (BT.available()) {
		trans = BT.read();
;
		if (trans == 1) {
			BT.println("LED ON");
			digitalWrite(LED, HIGH);
		}
		
		if (trans == 2) {
			digitalWrite(LED, LOW);
		}
	}

	else {
		BT.println("NO INPUT");
	}
	////TODO: FOR MEGA CHANGE TO :
	//// while(Serial.available() > 0){
	//while (Serial.available() > 0) {
	//	
	//	transmition += char(Serial.read());
	//	Serial.println(transmition);
	//}

	//if (transmition == "") {
	//	return;
	//}

	///*if(!transmition.equals(""))
	//	Serial.println(transmition);*/

	//if(transmition.equalsIgnoreCase("1")){
	//	digitalWrite(LED, HIGH);
	//	transmition = String("");
	//}

	//if (transmition.equalsIgnoreCase("0")) {
	//	digitalWrite(LED, LOW);
	//	transmition = String("");
	//}

}
