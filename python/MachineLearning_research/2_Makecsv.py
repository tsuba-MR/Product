"""
動作概要
1で作成したjsonファイルを読み込み、単語のみのデータに直してcsvファイルに書き出す
"""
import csv
import numpy as np
import pandas as pd
import glob
import os

#趣味の大元パス名指定        
path = 'jsonフォルダの絶対パス + \\'
bar = '-'
label = 0　#正例:0, 負例:1, 被験者:2とする

#JSONファイルから単語だけ抽出して出力
files = os.listdir(path) #1アカウント分の画像ファイル名のリスト作成
for filename in files:
    s = os.path.splitext(os.path.basename(filename))[0]#アカウント名
    #jsonファイルが存在したら実行する
    try:
        with open('%s%s.json'%(path,s)) as f:
            #descriptionを含む行だけ抜き取って、左右を消す
            l = f.readlines()
            d1 = [s for s in l if 'description' in s]
            d2 = [x.rstrip('",\n') for x in d1]
            d3 = [y.lstrip('            "description": "')for y in d2]
            #scoreを含む行だけ抜き取って、左右を消す
            sc1 = [s for s in l if 'score' in s]
            sc2 = [x.rstrip(', \n') for x in sc1]
            sc3 = [y.lstrip('            "score": ')for y in sc2]
    #jsonファイルが存在しなかった時はアカウント名を載せてエラーを吐く
    except FileNotFoundError as s:
        print("%sは存在しませんでした"%s)
        continue
    #csvファイルにdescriptionを書いていく
    with open ('directory-path\\%d%s%s.csv'%(label,bar,s),'w',newline='')as f:
        writer = csv.writer(f,lineterminator='\n')
        #スコアを引き算してマイナスになったら改行をしてユーザーを区別する
        for i in range(len(d3)):
            if float(sc3[i-1])-float(sc3[i])<0:
                writer.writerows([['\n']])
            writer.writerows([[d3[i]]])
