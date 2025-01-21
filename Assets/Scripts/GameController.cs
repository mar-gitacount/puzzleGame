using UnityEngine;

using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

public class GameController : MonoBehaviour

{
    private bool isStart;

    private List<GameObject> deleteList = new List<GameObject>();

    public GameObject[] Candies;

    //!セクションターンを作る。あとでインスタンスを利用して作成する。今は実数をいれておく。
    //! モンスターを作る。
    private List<testCharacter[]> characters = new List<testCharacter[]>();

    // ターンを設定する
    public int SectionTurn;




    //! セクションループを作る。
    //! 味方HP設定
    public int testAllyHp = 100;
    //! 敵HP設定
    public int testEnemyHp = 100;

    //配列の大きさを定義。

    private int width = 5;

    private int height = 7;

    //publicでGameObject型の配列を作る。

    public GameObject[,] candyArray = new GameObject[5, 7];

    // キャンディが3つ並んでいたら消去する。
    public void CheckMatching()
     {
        // 下の行から横のつながりを確認。
        foreach (var row in characters)
        {
            foreach (var character in row )
            {
                Debug.Log($"キャラクター:{character.Name}, HP:{character.Hp}");
            }
        }
        for(int i = 0; i<height; i++){
            // 右から二つめ以降は確認不要
            for(int j = 0;j<width-2;j++){
                //同じタグのキャンディが3つ以上ならんでいたら,X座標がJ
                  
                if((candyArray[j,i].tag == candyArray[j+1,i].tag) && (candyArray[j,i].tag == candyArray[j+2,i].tag))
                {
                    candyArray[j,i].GetComponent<CandyMove>().isMatching = true;
                    candyArray[j+1,i].GetComponent<CandyMove>().isMatching = true;
                    candyArray[j+2,i].GetComponent<CandyMove>().isMatching = true;
                    // !後で消す
                    testEnemyHp -= 10;
                    Debug.Log("敵残りHP" + testEnemyHp);
                }
            }
        }
        // 左の列から縦のつながりを確認する。
        for(int i = 0; i<width;i++)
        {
            // 上から2つ目以降は確認不要。Y方向に進む。
            for (int j = 0;j<height -2; j++)
            {

                if((candyArray[i,j].tag == candyArray[i,j + 1].tag) && (candyArray[i,j].tag == candyArray[i,j + 2].tag))
                {
                    candyArray[i,j].GetComponent<CandyMove>().isMatching = true;
                    candyArray[i,j+1].GetComponent<CandyMove>().isMatching = true;
                    candyArray[i,j+2].GetComponent<CandyMove>().isMatching = true;
                    // !後で消す
                    testEnemyHp -= 10;
                    // 敵のHPが0の時、次のセクションへ
                    Debug.Log("敵残りHP" + testEnemyHp);
                }
            }
        }
        // isMatching=trueのものをListに入れる。
        foreach(var item in candyArray)
        {
            if(item.GetComponent<CandyMove>().isMatching)
            {
                // 3つ以上揃った時、半透明にする。
                item.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.5f);
                // デリーとリストに入れる。
                deleteList.Add(item);
            }
        }
        if(deleteList.Count > 0)
        {
            Invoke("DeleteCandies",0.7f);
        }

     }
    
    void DeleteCandies()
      {
        foreach (var item in deleteList)
        {
            Destroy(item);
            // 消すキャンディの位置を取得し、そこを空にする。ここの場所がnullの場合、あとで追加される？
            candyArray[(int)item.transform.position.x,(int)item.transform.position.y] = null;
        }
        deleteList.Clear();
        Invoke("SpawnNewCandy",1.2f);
      }

    void Start()

    {
        // ゲームスタート
        // !テストコード
        // row = １クエストあたりのセクション数
         
        // 新しい行（配列）を作成して追加
        characters.Add(new testCharacter[] {
            new testCharacter("ドラゴン", 100),
            new testCharacter("ゴブリン", 50)
        }); // 0番目の行を追加

        characters.Add(new testCharacter[] {
            new testCharacter("エルフ",120)
        });//1番目の行を追加

        // モンスターリストを取得し、ターンを設定する。
        SectionTurn = characters.Count;
     
        CreateCandies();
        

    }


    // ゲームセクション管理関数
    void GemeSectionManagement()
    {
        // セクションを－1にする。
        // 次の配列の要素へいく。
        
    }


    void CreateCandies()

    {

        for (int i = 0; i < width; i++)

        {

            for (int j = 0; j < height; j++)

            {
                int r = Random.Range(0, 5);
                Debug.Log("This is an info message.");
                Debug.Log("Candies 配列のサイズ: " + Candies.Length);

                var candy = Instantiate(Candies[r]);
                //画面の見た目として、candyのtransform.positionを設定
                candy.transform.position = new Vector2(i, j);
                //画面に５×７の表があるイメージで、キャンディの座標をそのまま配列のIndexに利用して、配列の要素にCandyを入れている。
                candyArray[i, j] = candy;
            }

        }
        CheckStartset();

    }

    

void CheckStartset()

    {

        //下の行からヨコのつながりを確認

        for (int i = 0; i < height; i++)

        {

            //右から２つ目以降は確認不要（width-2）

            for (int j = 0; j < width-2; j++)

            {

                //同じタグのキャンディが３つ並んでいたら。Ｘ座標がｊなので注意。

　　　　//念のため、ふたつの式それぞれをカッコで囲んでいる。

                if ((candyArray[j,i].tag==candyArray[j+1,i].tag) && (candyArray[j, i].tag == candyArray[j + 2, i].tag))

                {

                    //CandyのisMatchingをtrueに

                    candyArray[j, i].GetComponent<CandyMove>().isMatching = true;

                    candyArray[j + 1, i].GetComponent<CandyMove>().isMatching = true;

                    candyArray[j + 2, i].GetComponent<CandyMove>().isMatching = true;

                }

            }

        }//

        //左の列からタテのつながりを確認

        for (int i = 0; i < width; i++)

        {

            //上から２つ目以降は確認不要。height-2

            for (int j = 0; j < height-2; j++)

            {

                //Ｙ座標がｊ。

                if ((candyArray[i,j].tag==candyArray[i,j+1].tag) && (candyArray[i,j].tag==candyArray[i,j+2].tag))

                {

                    candyArray[i, j].GetComponent<CandyMove>().isMatching = true;

                    candyArray[i, j+1].GetComponent<CandyMove>().isMatching = true;

                    candyArray[i , j+2].GetComponent<CandyMove>().isMatching = true;

                }

            }     

    }
            //isMatching=trueのものをＬｉｓｔに入れる

        foreach (var item in candyArray)

        {

            if (item.GetComponent<CandyMove>().isMatching)

            {

                deleteList.Add(item);

            }

        }

 

        //List内にキャンディがある場合

        if (deleteList.Count>0)

        {

            //該当する配列をnullにして（内部管理）、キャンディを消去する（見た目）。

            foreach (var item in deleteList)

            {

                candyArray[(int)item.transform.position.x, (int)item.transform.position.y] = null;

                Destroy(item);

 

            }

            //Listを空っぽに。

            deleteList.Clear();

  　//空欄に新しいキャンディを入れる。

            SpawnNewCandy();
        }
        else//Listにキャンディがない場合。

        {

            //ゲーム開始。

            isStart = true;

        }

 


      }

      //空欄に新しいキャンディを生成

    void SpawnNewCandy()

    {

        

        for (int i = 0; i < width; i++)

        {

            for (int j = 0; j < height; j++)

            {

                if (candyArray[i, j] == null)

                {

                    int r = Random.Range(0, 5);

                    var candy = Instantiate(Candies[r]);

　　　　　//見た目の処理

                    candy.transform.position = new Vector2(i, j+0.3f);

                   //内部管理の処理

             candyArray[i, j] = candy;

                }

            }

        }
        if (isStart == false)

        {

            CheckStartset();

        }
        else
        {
            foreach(var item in candyArray)
            {
                int column = (int)item.transform.position.x;
                int row = (int)item.transform.position.y;
                item.GetComponent<CandyMove>().myPreviousPos = new Vector2(column,row);

            }
            // 続けざまに3つそろっているかどうか判定
            Invoke("CheckMatching",0.2f);
        }

    }

}

public class testCharacter
{
    public string Name { get; set; }
    public int Hp { get; set; }

    public testCharacter(string name, int hp)
    {
        Name = name;
        Hp = hp;
    }
}