"""
動作概要
作成したデータセットから機械学習を行い、正解率を出力する
本ファイルの段階でデータセットの編集やパラメータチューニングを行う
今回は1例として
・          データセット:最小値0~最大値1となるよう正規化
・パラメータチューニング：なし
の組み合わせとした
"""
#学習関連
import xgboost as xgb
from sklearn import model_selection
#評価関係
from sklearn import metrics
from sklearn.metrics import confusion_matrix, classification_report,make_scorer,recall_score
from sklearn.model_selection import cross_validate,KFold ,StratifiedKFold
#データ分割用
from sklearn.model_selection import train_test_split
#データを扱うためのpandas
import pandas as pd
import numpy as np
#時間計測
import time
import pprint

#csv読み込み
df = pd.read_csv('Vector_syumi_4_ramen.csv', index_col=0)
df = df.fillna(0)
df = df.loc[:, 'Answer':'finish_word']#特徴量の「Answer」から「finish_word」まで指定
df = df[:182]#トレーニングデータと被験者の計181行を読み込み

#機械学習を行うためのデータ作成
#トレーニングデータと被験者（テストデータ）、ラベルの追加を行う
X = df
data_train = X.iloc[0:130]#トレーニングデータ
data_hiken = X.iloc[130:181]#被験者。研究時は51人
y = data_train['Gender']#トレーニングデータのラベルを追加
X_train = data_train.drop('Gender', axis=1)

#出現頻度を0~1のデータに直す
X2 = ((X_train.T - X_train.T.min()) / (X_train.T.max() - X_train.T.min())).T

#特異度の計算
def specificity_score(y_true, y_pred):
    tn, fp, fn, tp = confusion_matrix(y_true, y_pred).flatten()
    return tn / (tn + fp)
#研究で使った独自の正解率の計算
def A_score(y_true, y_pred):
    tn, fp, fn, tp = confusion_matrix(y_true, y_pred).flatten()
    return (tn/(tn+fp)+tp/(tp+fn))/2

#学習を行い、5交差検証をして正解率を計算する
y_true = []
#xgboostモデル作成
clf = xgb.XGBClassifier()
clf.fit(X2, y)
kf = StratifiedKFold(n_splits=5,shuffle=True,random_state=0)

for train_index,test_index in kf.split(X2,y):
    print("train_index:",y.iloc[train_index])
    print("test_index",y.iloc[test_index])

scoring = {
    'recall':make_scorer(recall_score),
    'specificity':make_scorer(specificity_score),
    'A':make_scorer(A_score),
}

scores_kf = cross_validate(clf,X2,y,cv=kf,n_jobs = 1,scoring = scoring)

print("クラス確認")
print(clf.classes_)

print("交差検証の結果")
pprint.pprint(scores_kf)
