"""
動作概要
チャンネル名をすべて小文字アルファベットに統一し、アルファベットの出現回数をカウントする
動作不良となるチャンネル名が存在するため、そういったチャンネルは例外処理で省く
"""
import pandas as pd
import datetime
from pykakasi import kakasi

dt_now  = datetime.datetime.now()
month = str(dt_now.month).zfill(2)#一桁付きは0を先頭につける

#日本語変換用
kakasi = kakasi()
kakasi.setMode('H', 'a')
kakasi.setMode('K', 'a')
kakasi.setMode('J', 'a')

conv = kakasi.getConverter()

df = pd.read_csv('kakasi_csv_only_name_ytb_str_{}.csv'.format(month),index_col=0)

#リスト作成
#nameリストを作成。name[i]で指定できる
name = df.index.values
print("name:{}".format(name))
length = []
alphabet = []

#アルファベットリスト作成用
for i in range(97, 123):
    alphabet.append(chr(i))

#bag-of-alphabet2次元リスト作成
boa = [[0]*len(alphabet) for l in range(len(name))]


for i in range(len(name)):#行
    try:
        v = conv.do(name[i]).lower()#小文字にそろえる
    except TypeError:
        print("{0}番目の{1}がうまくできませんでした".format(i,name[i]))
        pass
    v2 = v.replace(" ","")#空欄あったら削除
    v3 = v2.replace(".","")#空欄あったら削除
    length.append(len(v3))#文字の長さリストに各VTuberの長さを入れる
    chl = list(v3)#アルファベット分割
    for j in range(len(chl)):#列
        for k in range(len(alphabet)):#名前のアルファベット出現回数
            if chl[j] == alphabet[k]:#名前のアルファベットが存在した分だけ
                boa[i][k] += 1#アルファベット出現回数を1増加させる


df2 = pd.DataFrame(boa, index=name, columns=alphabet)
df2.insert(0,"Length",length)#文字数カウントを追加
df2
df2.to_csv('kakasi_vector_name_ytb_str_{}.csv'.format(month),encoding="utf_8_sig")

print("done")