"""
動作概要
特定のメッセージを受け取ると対応するコマンドを実行する
・使ったキャラクターの履歴を表示する
・試合進行状況をgoogle spreadsheetに自動で記述する
"""
import discord
import settings2
import random
import gspread
import json
import time
import sys

from oauth2client.service_account import ServiceAccountCredentials

#Botのアクセストークン
TOKEN = settings2.TOKEN
CHANNEL_ID = settings2.CHANNEL_ID
SPREADSHEET_KEY = settings2.SPREADSHEET_KEY

#2つのAPIを記述しないとリフレッシュトークンを3600秒毎に発行し続けなければならない
scope = ['https://spreadsheets.google.com/feeds','https://www.googleapis.com/auth/drive']

#認証情報設定
#秘密鍵を読み込む
credentials = ServiceAccountCredentials.from_json_keyfile_name('secret-key.json', scope)
#これもsettingにファイル名を入れておく必要あり

#OAuth2の資格情報を使用してGoogle APIにログイン
gc = gspread.authorize(credentials)


#接続用のオブジェクトを生成
client = discord.Client()


#起動時の動作
@client.event
async def on_ready():
    print('We have logged in as {0.user}'.format(client))
    #await greet()
#メッセージ受信時の動作
@client.event

async def on_message(message):
    
    # メッセージの送信者がbotだった場合は無視する
    #どのファイターを使ったか確認するコマンド       
    if message.content == '/clist':
        #IDでシートを検索する
        worksheet = gc.open_by_key(SPREADSHEET_KEY).worksheet("Character")
        id = message.author.id#メッセージ送信者のID取得
        cell = worksheet.find("%d"%(id))#IDを検索
        #await message.channel.send(cell.col)#IDがあるセルの列番号を取得
        col_list = worksheet.col_values(cell.col)#該当セルの列番号を取得しリスト作成
        col_show = col_list#キャラ表示用にリスト複製
        len_list = len(col_list)
        await message.channel.send('使用済みファイターはこちらです')
        col_show.pop(0)
        col_show2 = "\n".join(col_show)#改行して表示
        if len_list == 1:
            await message.channel.send("無し")
        else:     
            await message.channel.send(col_show2)
        
   
    
    #トーナメントの勝敗を管理するコマンド
    if message.content == '/tnam':
        try:
            num = int(message.channel.name)#試合番号名を受け取り、それをint型に変換
        except ValueError:
            await message.channel.send('エラー！エラー！')
            await message.channel.send('該当のトーナメント番号が書いてあるチャンネルに移動してください')
            
        worksheet = gc.open_by_key(SPREADSHEET_KEY).worksheet("tournament")
        status = worksheet.cell(3*num, 1).value#試合状況の確認
        #トーナメント番号にいるか確認
        
        await message.channel.send('試合番号は%dです'%(num))
        cell_value = int(worksheet.cell(3*num-2, 6).value)#何試合目かを取得
        await message.channel.send('セットカウントは%dです'%(cell_value))
        
        if status != "Incompleteing":
            await message.channel.send('試合はすでに修了しています')
        else:
            await message.channel.send('組み合わせが上の人が入力してください\n「勝ち」ましたか？「負け」ましたか？')
            #文章が"こんにちは"かつ送信チャンネルが同じならTRUE
            try:
                result = await client.wait_for("message", timeout=60.0)#この関数は複数人同時で実行できないから単純にwhileループにすべき
            except asyncio.TimeoutError:
                await channel.send('時間切れです。')
            #試合がすでに終了していたら抜けるようにする必要がある
         
            if result.content == "勝ち":
                oorx = "O"
            elif result.content == "負け":
                oorx = "X"    
            else:
                await message.channel.send('結果をもう一度入力してください')
                
            if (status == "Incompleteing"):
                worksheet.update_cell(3*num-2,cell_value+1,oorx)#試合番号によって、トナメ上側の人の結果を記入
                #GASとPythonに書き込む
                await message.channel.send('記入しました。結果を確認します')
            else:
                await message.channel.send('試合はすでに修了しています')
            
            time.sleep(2)
            status = worksheet.cell(3*num, 1).value#試合状況の更新
            
                
            if (status == "Incompleteing"):
                await message.channel.send('試合は継続中です')
            else:
                await message.channel.send('試合終了、買ったのは%sです'%(worksheet.cell(3*num, 1).value))

        await message.channel.send('done')
     




    
#BOTの起動とDiscordサーバーへの接続        
client.run(TOKEN)
# In[ ]:

