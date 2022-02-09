void setup() {
  // 伝送速度と入力ピンの設定
  Serial.begin(9600);
  pinMode(2, INPUT);
}

void loop() {
  // 2ピンの状態を通信
  Serial.println(digitalRead(2));
  //delay(1000);
}
