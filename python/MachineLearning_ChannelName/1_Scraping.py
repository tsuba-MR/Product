"""
動作概要
ユーチュラというサイトの情報を収集する
"""
import requests
import datetime
import time

dt_now  = datetime.datetime.now()

year = str(dt_now.year)
month = str(dt_now.month).zfill(2)#一桁付きは0を先頭につける
print(month)


#ユーチュラのランキング最後尾まで集める。日々増加するので適宜更新
end_page = 2000


#end_pageまでデータを集める
#ファイル名は4桁になるように指定して、各ページのソースコードを保存する
for i in range(1,end_page):
    time.sleep(1)
    if i != 1:
        get_url_info = requests.get('https://ytranking.net/ranking/?p={0}&mode=view&date={1}{2}'.format(i,year,month))
    else:
        get_url_info = requests.get('https://ytranking.net/ranking/?mode=view&date={0}{1}'.format(year,month))
    fname = str(i).zfill(4)
    with open('ytbrank_str/{}.txt'.format(fname),'wb')as file:
        for chunk in get_url_info.iter_content(4029):
            file.write(chunk)