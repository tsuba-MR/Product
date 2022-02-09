/*
Chatter.cpp - Library for Tact switch code.
  Created by Tsubasa Maruyama, October 17, 2021.
  Released into the public domain.
*/

#include "Arduino.h"
#include "Chatter.h"

Chatter::Chatter(int pin){
  pinMode(pin, INPUT_PULLUP);
  _pin = pin;
}

void Chatter::chattering(int &count, int &rate){
  if(digitalRead(_pin) == 1){  //押してる時
    if(count < rate)
    {
      Serial.print(0);
      count++;
    }
    else if(count >= rate){
      Serial.print(1);
    }
  }else{   //押してないとき
    count = 0;
    Serial.print(0);
  }
}