using UnityEngine;
using System.Collections.Generic;
// using System.Numerics;

public class CandyMove : MonoBehaviour

{
    // マウスの位置
    private Vector2 previousMousePosition;
    // ドラッグフラグ
    private bool isDragging = false;

    // クリックフラグ
    private bool isClicked = false;

    //GameControllerスクリプトを使うので、指定する。

    private GameController gameControllerCS;

　//自身の入っている配列の座標

    public int column;//列

    public int row;//行

    //スワイプしたときの座標を確認するための変数

    private Vector2 fingerDown;

    private Vector2 fingerUp;

    private Vector2 distance;
    private Vector2 currentMousePosition; // 現在のマウス位置

    //隣のキャンディ

    private GameObject neighborCandy;

　//３つ並んでいるとき知らせる

    public bool isMatching;

    //移動前の座標

    public Vector2 myPreviousPos;

    // スワイプフラグ
    private bool hasSwipe = false;

    // 今選択されているキャンディ
    private static CandyMove selectedCandy = null;

    



 

void Start()

    {

        if(Camera.main == null)
        {
            Debug.LogError("メインカメラが存在しません");
        }else
        {
            Debug.Log("メインカメラは動作している。");
        }
       gameControllerCS = Object.FindFirstObjectByType<GameController>();
        Application.targetFrameRate = 60; // フレームレートを60FPSに設定


        //自分の位置を座標配列の番号（Index)にあてておく。

        column = (int)transform.position.x;

        row = (int)transform.position.y;

　　//スタート位置を記録する。

        myPreviousPos = new Vector2(column,row);

        

    }


 

 //指をおいたとき

    private void OnMouseDown()

    {
        if(isClicked)
        {
            Debug.Log($"クリック中{previousMousePosition}");
            return;
        }
        //! fingerDown = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //! previousMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isClicked = true;
        // ここでクリックした場所を代入してみる。

        //! Debug.Log($"ドラッグ中座標を変える！！指をおろした場所:{previousMousePosition}");
        hasSwipe = false;
        // スワイプする。
        // SwapCandies(neighborCandy);


    }

    private void OnMouseDrag()
    {
        // currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // distance = currentMousePosition - fingerDown;

        // オブジェクトをリアルタイムで動かす。
        // transform.position += (Vector3)distance;

        // 現在位置を次の基準点として更新

        // transform.position += (Vector3)distance;
        if(distance.magnitude > 0.5f)
        {
            // moveCandies();
            // hasSwipe = true;
        }

        // fingerDown = currentMousePosition;
        // Debug.Log($"ドラッグ中: 距離 {distance}、位置 {transform.position}");
        // Debug.Log($"ドラッグ中: 更新前 {fingerDown}、更新後 {currentMousePosition}");
       
    }

    //指を離したとき
    private void OnMouseUp()

    {
        isDragging = false;
        fingerUp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //２点のベクトルの差を計算
        Debug.Log($"指を下げた座標{fingerDown}指を上げた座標{fingerUp}");

        // distance = fingerUp - fingerDown;
        hasSwipe = false;
        // moveCandies();


    }
    void moveCandies()

    {
        //! 思いつきだが、直接指の位置を代入する。

        //右にスワイプしていたなら。（Mathf.Absとは絶対値を示す）
        if (distance.x>=0 && Mathf.Abs(distance.x)>Mathf.Abs(distance.y))
        {

            //自身が一番右にいない場合、となりのキャンディと位置を交換する
            if (column<4)
            {
                //右隣りのキャンディ情報をneighborCandyに代入
                // ここで、となりのいった情報も入れる？
                neighborCandy = gameControllerCS.candyArray[column + 1, row];
                Debug.Log($"スワイプ中!!");
                
                // スワイプする。
                // SwapCandies(neighborCandy);

                //隣のキャンディを１列左へ。
                // これを座標を直接指定する？
                
                neighborCandy.GetComponent<CandyMove>().column -= 1;

                //自身は１列右へ。

                column += 1;

            }

        }

        //左にスワイプしていたなら。

        if (distance.x < 0 && Mathf.Abs(distance.x) > Mathf.Abs(distance.y))

        {

            //自身が一番左にいない場合、となりのキャンディと位置を交換する

            if (column > 0)

            {

                //左隣りのキャンディ情報を取得

                neighborCandy = gameControllerCS.candyArray[column - 1, row];

                

                //隣のキャンディを１列右へ。

                neighborCandy.GetComponent<CandyMove>().column += 1;

                //自身は１列左へ。

                column -= 1;

            }

        }

        //上にスワイプしていたなら。

        if (distance.y >= 0 && Mathf.Abs(distance.x) < Mathf.Abs(distance.y))

        {

            //自身が一番上にいない場合、となりのキャンディと位置を交換する

            if (row < 6 )

            {

                //上のキャンディ情報を取得
                neighborCandy = gameControllerCS.candyArray[column, row+1];
                //隣のキャンディを１行下へ。
                neighborCandy.GetComponent<CandyMove>().row -= 1;

                //自身は１行上へ。

                row += 1;

            }

        }

        //下にスワイプしていたなら。

        if (distance.y < 0 && Mathf.Abs(distance.x) < Mathf.Abs(distance.y))

        {

            //自身が一番下にいない場合、となりのキャンディと位置を交換する

            if (row > 0)

            {

                //下のキャンディ情報を取得

                neighborCandy = gameControllerCS.candyArray[column, row - 1];

           

                //隣のキャンディを１行上へ。

                neighborCandy.GetComponent<CandyMove>().row += 1;

                //自身は１行下へ。

                row -= 1;

            }


        }
        Invoke("DoCheckMatching",0.5f);
        

    }

        public void SetCandyToArray()

    {

        gameControllerCS.candyArray[column, row] = gameObject;

    }
    void Update()

    {
        // タップ状態の管理テストコード、スマホ用
        if(Input.touchCount>0){
            Touch touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                Debug.Log($"指が画面に置かれています！現在の位置: {touch.position}");
            }
        }
        if (Input.GetMouseButton(0)) // デスクトップ用クリック時
        {
            // !テストコードここから
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Camera.main == null)
            {
                Debug.Log("メインカメラが設定されていない");
            }
            if(Physics.Raycast(ray, out hit))
            {
                // レイが当たったオブジェクトを取得する。
                CandyMove clickedCandy = hit.collider.GetComponent<CandyMove>();
                Debug.Log($"クリック時の場所{clickedCandy}");
                if(clickedCandy != null)
                {
                    if (selectedCandy != null)
                    {
                        Debug.Log($"他のキャンディが選択された。");
                    }
                    Debug.Log($"クリック時の場所{clickedCandy}");
                }

            }
            // !テストコードここまで


            if(isClicked)
            {
                // !一回目は、クリックした場所をいれる仕様にする。
                isDragging = true;
                previousMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Debug.Log($"マウスが押されてます{previousMousePosition}");
            }


        }
        if (Input.GetMouseButtonUp(0))
        {
            // ドラッグ解除
            isDragging = false;
            isClicked = false;
            Debug.Log("マウスが離されました");
            // moveCandies();
        }
        // ! ここで、マス7×5＝35回行われる
        if(isDragging)
        {
            // 指が置かれた座標。
            Vector2 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // Vector2 targetPosition = previousMousePosition + distance;
            // スムーズに追従
            // transform.position = Vector2.Lerp(transform.position, targetPosition, 0.3f);
            

            // ここで一回のタップをoffにする。
            isClicked = false;
            // currentMousePosition.z = 0; // Z座標を固定
            // 座標があってるか？
            if ((currentMousePosition - previousMousePosition).magnitude > 0.5f)
            {
                // 指の位置と、既存の位置の差。
                distance = currentMousePosition - previousMousePosition;
                moveCandies();
                // 前のポジションをいれる。
                previousMousePosition = currentMousePosition;
                // isDragging = false;
                // isClicked = false;
            }
        }
        //現在の座標と、column、rowの値が異なるとき。
        if (transform.position.x!=column || transform.position.y!=row)
        {
　　        //column,rowの位置に徐々に移動する。
            transform.position = Vector2.Lerp(transform.position, new Vector2(column, row), 0.3f);
            Debug.Log("移動テスト");
　　        //現在の位置と、目的地(column,row)との距離を測る。
            Vector2 dif = (Vector2)transform.position - new Vector2(column, row);
　　　　     //目的地との距離が0.1fより小さくなったら。
            if (Mathf.Abs(dif.magnitude)<0.1f)
            {

                transform.position = new Vector2(column, row);
                //自身をCandyArray配列に格納する。
               SetCandyToArray();
            }

        }     //自分が０行目（一番下）ではなく、かつ、下にキャンディがない場合、落下させる

        else if (row>0 && gameControllerCS.candyArray[column,row-1]==null)

        {

            FallCandy();

        }

    }

    void FallCandy()

    {

        //自分のいた配列を空にする

        gameControllerCS.candyArray[column, row] = null;

        //自分を下に移動させる

        row -= 1;

    }

    void DoCheckMatching()
    {
        if(isDragging)
        {
            return;
        }
        gameControllerCS.CheckMatching();
    }

}