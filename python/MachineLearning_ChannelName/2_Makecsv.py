"""
動作概要
チャンネル名を含む行のみ抜き取り、不要な情報を取り除く。チャンネル名のみの情報をcsvにまとめる
"""
import csv
import numpy as np
import pandas as pd
import glob
import os
import re
import datetime


dt_now  = datetime.datetime.now()
month = str(dt_now.month).zfill(2)#一桁付きは0を先頭につける
vn = []

#ソースコードを保存したディレクトリのパス名
path = "C:\\"#指定したディレクトリに保存
    
files = os.listdir(path)
#編集無しでnameファイルを作成する。
for filename in files:
    s = os.path.splitext(os.path.basename(filename))[0]
    #print("ファイルの名前は%s"%(s))
    with open("{0}{1}.txt".format(path,s),encoding="utf-8")as f:
        line = f.readlines()#行リスト
        for i in range(len(line)):
            if '<p><i class="material-icons" title="チャンネル登録者数"' in line[i]:
                vn.append(line[i-1])
        vn2 = [x.lstrip('\t<p class="title">') for x in vn]
        vn3 = [x.rstrip('</p>\t<aside>\n') for x in vn2]
        
df = pd.DataFrame(index=vn3)
df.to_csv('kakasi_csv_only_name_ytb_str_{}.csv'.format(month),encoding="utf_8_sig")
print("done")