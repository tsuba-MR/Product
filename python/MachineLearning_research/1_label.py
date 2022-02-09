"""
動作概要
写真をユーザー毎まとめたディレクトリを指定して、写真を16枚単位でVisionAPIにリクエストする
返ってきたレスポンスをjsonファイルに格納する
"""
from base64 import b64encode
import json
import requests
import glob
import time
import os

#時間測定開始
t1 = time.time() 

#エンドポイントやAPIキーの入力
ENDPOINT_URL = 'https://vision.googleapis.com/v1/images:annotate'
api_key = 'APIKey'

#親ディレクトリ一覧パス
path_dir = 'path'
#アカウント名リスト作成
folds = os.listdir(path_dir) 

#アカウント毎リクエストしていく
for foldname in folds:
    path = path_dir+foldname+"\\"#各アカウントのパス名
    filenames = os.listdir(path) #1アカウント分の画像ファイル名のリスト
    num_files = len(filenames) #1アカウント分の画像ファイル数
    
    #1アカウントの画像ファイルを16枚ずつに分けたとき何roop分あるか
    #GCPのリクエストが1リクエスト当たり最大16枚
    roop = num_files // 16
    if num_files % 16 != 0:
        roop += 1
        
    if num_files != 0:#フォルダ内の画像が0でないもののみ有効
        for i in range(roop): #iループ目
            img_requests = [] #requestのリスト
            for j in range(16): #16枚のうちj枚目の画像ファイル
                with open(path+filenames[j+16*i], 'rb') as f:
                    ctxt = b64encode(f.read()).decode()#読み込んだ画像をbase64エンコード
                    img_requests.append({
                            'image': {'content': ctxt},#画像のデータを示す。base64エンコードした画像データをcontentで指定する
                            'features': [{#解析に利用するツールの指定
                                'type': 'LABEL_DETECTION',#label_detectionを使用する
                                'maxResults': 100#上から順番に100個表示(全ての単語データを使用する)
                            }]
                    })
                #iループ目の中でファイル数が16枚に達しないときはbreak
                if filenames[j+16*i] == filenames[-1]:
                    break
            #APIのリクエスト先に、data,params,headerでリクエストする
            response = requests.post(ENDPOINT_URL,
                                     data=json.dumps({"requests": img_requests}).encode(),
                                     params={'key': api_key},
                                     headers={'Content-Type': 'application/json'})

            #jsonファイル作成
            for idx, resp in enumerate(response.json()['responses']):
                with open('directory-path%s.json'%foldname, 'a') as f:
                    json.dump(resp, f, indent = 4)

t2 = time.time() #時間測定修了
#計測時間
elapsed_time = t2 - t1
print("Time:")
print(elapsed_time)
