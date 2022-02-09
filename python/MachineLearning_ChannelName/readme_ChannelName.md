## MachineLearning_ChannelName
> バージョン<br>
Python 3.8.3<br>
> 使用ライブラリ<br>
requests, pandas, pykakasi, scikit-learn, XGboost<br>
## ファイルの説明
- 1_Scraping.py<br>
ユーチュラというランキングサイトから，YouTuberのチャンネル名が記載されたWebページのソースコードを収集します．<br>
ランキングは日々変わっていくため，月ごとデータを分けて情報を保存できるようにしました．<br>
- 2_Makecsv.py<br>
前項で集めたソースコードからチャンネル名だけを抜き取り，情報をcsvファイルに保存します．<br>
こちらも月ごとデータを分けて保存し，データを比較することができるようにしました．<br>
- 3_MakeVector.py<br>
pykakasiというライブラリを使用してチャンネル名をすべてアルファベットに変換した後，各文字の出現頻度を特徴量として機械学習に必要なデータセットを作成します．<br>
中には使用できないチャンネル名があったため，これは例外処理で省き正常にデータセットが作成できるようにしました．<br>
- 4_MachineLearning.py<br>
前項で作成したデータセットを基に機械学習を行い，正解率とF値を5分割交差検証で算出します．<br>
5分割交差検証によって本モデルの汎化性能を出すことができます．<br>
- Onomancy of YouTuber Channnel Name.ipynb<br>
上記4ファイルをまとめてjupyter notebook上で扱えるようにしたファイルです．<br>
