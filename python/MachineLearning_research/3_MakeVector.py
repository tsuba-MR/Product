"""
動作概要
2で作成したcsvファイルを読み込み、単語の出現頻度をまとめて自動的にデータセットを作成する
"""
import pandas as pd
import numpy as np
import csv
import glob
import os
import time

#時間計測
t1 = time.time()


data = [] #出現単語リスト
filename = [] #ファイル名リスト

#CSVファイルの読み込み
files = glob.glob("directory-path\\*.csv")
for file in files:
    with open(file) as f:
        name = os.path.splitext(os.path.basename(file))[0] #ファイル名の取得
        filename.append(name) #ファイル名をファイル名リストに追加
        #リストの初期化
        d = []
        d.clear()
        df_nan_set_na = pd.read_csv(file,  na_values='\n') #改行をNaNに
        df = df_nan_set_na.dropna() #NaNの行は削除
        d = list(df.iloc[:, 0]) #リスト化
        data.append(d) #出現単語リストにリストを追加
    f.close()
    
#特徴量となる単語の名簿リストを作る
feature = [] #特徴量リスト
for i in range(len(data)):
    for j in range(len(data[i])):
        if data[i][j] not in feature: #まだ見つかっていない特徴量なら
            feature.append(data[i][j]) #feature末尾に特徴量追加

#行：dataの長さ、列：特徴量の長さ、の空の二次元配列を作成、
frequency = [[0] * len(feature) for l in range(len(data))]

#対応する特徴量の出現頻度をカウントする
for i in range(len(data)):
    for j in range(len(data[i])):  
        #data[i][j]と一致するfeature[k]を見つける
        for k in range(len(feature)): 
            if data[i][j] == feature[k]:
                frequency[i][k] += 1 #frequency[i][k]をインクリメント：出現頻度の値を増やす
print(frequency)

#csvファイルに書き込み
df_w = pd.DataFrame(frequency, index=filename, columns=feature)
df_w.to_csv("Vector_hobbyname.csv")

#時間計測
t2 = time.time()
elapsed_time = t2 - t1
print(elapsed_time)
