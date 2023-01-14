//skripta na transmiteru
#include<SPI.h>
#include<nRF24L01.h>
#include<RF24.h>

char inByte;
RF24 radio(9, 10); //CE,CSN

const byte address[6] = "00001";

void setup() {
  // put your setup code here, to run once:
  Serial.begin(9600);
  radio.begin();
  radio.openWritingPipe(address);
  radio.setPALevel(RF24_PA_MIN);
  radio.stopListening();

}

void loop() {
  delay(5);

  //procitaj podatke koje primis od Unity-ja
  if(Serial.available() > 0)
  {
      inByte = Serial.read();
  }

  //salji podatke koje primis do fizickog auta
  radio.write(&inByte, sizeof(inByte));
  
  delay(1000);
}
