## Shader_Raymarching
> バージョン
Unity 2020.3.7f1
> ファイル，ディレクトリの説明
### Code
- Marching_Torus.shader
リング状のプリミティブを複製し、浮遊しているように見せるレイマーチングの作品です。
フラグメントシェーダーで距離関数などを用いてプリミティブを表示しています。
動きがつくように、時間経過で上下移動や回転を行うようにしました。
- sm_10.cs
バウンディングボックスを巨大に拡張させたメッシュを作成するためのファイルです。
カメラを移動させたときにQuadが視錐台から外れて映像が表示されなくなる問題があります。
そこで、バウンディングボックスを拡張したメッシュを生成しそれをQuadのMesh Filterにアタッチすることでカメラがどれだけ動いても映像が表示されるようになります。

### UnityPackage
- Shader_pmCryastal.unitypackage
本製作の情報をまとめたunitypackageです。