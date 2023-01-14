//skripta na autu
#include <RF24.h>
#include <SPI.h>
#include <nRF24L01.h>

//vrijeme u ms koliko ce se fizicki auto kretati
#define FORWARD_ONE_TILE 250
#define TRUN_90_DEGREES 150
#define CAR_SPEED 50


//nRF24 configuration
RF24 radio(9, 10); // CE, CSN
const byte address[6] = "00001";
char inByte;

// L298N DC motor driver dual H-bridge
// defining pins for left motors

void setup() {
    Serial.begin(9600);
    // setting all L298N pins as output
    pinMode(enA_1, OUTPUT);
    pinMode(enB_1, OUTPUT);
    pinMode(in1_1, OUTPUT);
    pinMode(in2_1, OUTPUT);
    pinMode(in3_1, OUTPUT);
    pinMode(in4_1, OUTPUT);
    robotStop();
    
    // nRF24 initialization
    radio.begin();
    radio.openReadingPipe(0, address);
    radio.setPALevel(RF24_PA_MIN);
    radio.startListening();
}

void loop() {
  
    // getting data/inputs from nRF24
    if (radio.available()) {
        radio.read(&inByte, sizeof(inByte));
    }

    //u daljnim if-ovima se provjerava input
    //naprijed
    if (inByte == 'W') {
        Serial.println("Forward");
        goForward();
    }

    //okreni se lijevo
    if (inByte == 'A') {
        Serial.println("Left 90");
        turnLeft();
    }

    //okreni se desno
    if (inByte == 'D') {
        Serial.println("Right 90");
        //inByte = "";
        turnRight();
    }

    //okreni se za 180
    if (inByte == 'S') {
        Serial.println("Turn 180");
        turn180Degrees();
    }
}

void goForward() {
    digitalWrite(in1_1, HIGH);
    digitalWrite(in2_1, LOW);
    digitalWrite(in3_1, HIGH);
    digitalWrite(in4_1, LOW);
    analogWrite(enB_1, CAR_SPEED);
    analogWrite(enA_1, CAR_SPEED);
    
    delay(FORWARD_ONE_TILE);
    robotStop();
}



void turnLeft() {
    digitalWrite(in1_1, LOW);
    digitalWrite(in2_1, LOW);
    digitalWrite(in3_1, HIGH);
    digitalWrite(in4_1, LOW);
    analogWrite(enB_1, CAR_SPEED);
    analogWrite(enA_1, 0);
    delay(TRUN_90_DEGREES);
    robotStop();
}

void turnRight() {
    digitalWrite(in1_1, HIGH);
    digitalWrite(in2_1, LOW);
    digitalWrite(in3_1, LOW);
    digitalWrite(in4_1, LOW);
    analogWrite(enB_1, 0);
    analogWrite(enA_1, CAR_SPEED);
    delay(TRUN_90_DEGREES);
    robotStop();
}

void turn180Degrees(){
    digitalWrite(in1_1, HIGH);
    digitalWrite(in2_1, LOW);
    digitalWrite(in3_1, LOW);
    digitalWrite(in4_1, HIGH);
    analogWrite(enB_1, CAR_SPEED);
    analogWrite(enA_1, CAR_SPEED);
    delay(TRUN_90_DEGREES*2);
    robotStop();
 }

void robotStop() {
    
    digitalWrite(in1_1, LOW);
    digitalWrite(in2_1, LOW);
    digitalWrite(in3_1, LOW);
    digitalWrite(in4_1, LOW);

    analogWrite(enA_1, 0);
    analogWrite(enB_1, 0);
}
