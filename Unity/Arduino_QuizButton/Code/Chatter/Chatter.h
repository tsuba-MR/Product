/*Chatter.h - Library for Tact switch code.
  Created by Tsubasa Maruyama, October 17, 2021.
  Released into the public domain.*/

#ifndef Chatter_h
#define Chatter_h
#include "Arduino.h"

class Chatter{
  public:
    Chatter(int pin);
    void chattering(int &count, int &rate);
  private:
    int _pin;
};

#endif