"""
動作概要
作成したデータセットを使って機械学習を行い、5交差検証で正解率とF値を算出する
元のデータセットから上1万行、下１万行を抽出して学習させる
データセットの正規化処理とパラメータチューニングの組み合わせが、
各行が単位ベクトルになるよう正規化×チューニングなし
で最もスコアが高かったため、この組み合わせを継続して値を確認している
"""
#ライブラリのインポート
import xgboost as xgb
from sklearn import model_selection
#ハイパーパラメータ探索のため、ランダムサーチ
from sklearn.model_selection import RandomizedSearchCV
from scipy import stats
#評価関係
from sklearn import metrics
from sklearn.metrics import confusion_matrix, classification_report,make_scorer,accuracy_score,f1_score,matthews_corrcoef
#データ分割用
from sklearn.model_selection import train_test_split,StratifiedKFold,cross_validate
from sklearn.utils.fixes import loguniform
#データを扱うためのpandas
import pandas as pd
import scipy as sp
import numpy as np
#時間計測
import time
import pprint


#特徴ベクトルを整理
import datetime
dt_now  = datetime.datetime.now()
month = str(dt_now.month).zfill(2)#1~9月は0を先頭につける
df = pd.read_csv("kakasi_vector_name_ytb_str_{}.csv".format(month),index_col=0)
#上下10000個だけ抽出
df_head=df[0:10000]
df_tail=df[-10001:-1]
df_marge=pd.concat([df_head,df_tail])
df_marge
df=df_marge

#ラベル付け。半分で分けて0,1でつける
a = len(df)#行の長さを取得
if(a % 2 == 0):
    label = [0]* int(a/2) + [1] * int(a/2)
else:
    label = [0]* int(a/2 + 1) + [1] * int(a/2)
y = label
#ベクトル正規化
X = X1.apply(lambda x:x/np.sqrt(sum(x**2)), axis=1)
#0-1正規化
#X= ((X1.T - X1.T.min()) / (X1.T.max() - X1.T.min())).T
X_train, X_test, y_train, y_test = train_test_split(X, y, test_size=0.3, random_state=0)


#5交差検証
start = time.time()
y_true = []
#学習
clf = xgb.XGBClassifier()
clf.fit(X,y)
kf = StratifiedKFold(n_splits=5,shuffle=True,random_state=0)

scoring={
    "accuracy":make_scorer(accuracy_score),
    "f1_score":make_scorer(f1_score)
}    
scores_kf = cross_validate(clf,X,y,cv=kf,n_jobs=1,scoring = scoring)
print("5交差検証の結果")
pprint.pprint(scores_kf)
elapsed_time = time.time() - start
print(f'Time : {elapsed_time}')