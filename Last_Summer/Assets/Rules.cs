using UnityEngine;
using System.Collections;

public class Rules : MonoBehaviour 
{
	public static bool turn = true; //true is white
	public static int[,] ref_board = new int[8,8]; //a matrix with 1,-1, and 0 for reference
	public GameObject white1;
	public GameObject black1;
	public static Object[,] real_board = new Object[8, 8]; //a matrix with gameobjects as a mirror. 
	public Vector2 white_pos;

	void Start () 
	{
		white1 = GameObject.Find("white");
		black1 = GameObject.Find("black");

		ref_board [3,3] = -1; //-1 is black
		ref_board [3,4] = 1;  //1 is white    2 is slated for demolition
		white_pos = new Vector2(3, 4);
		Place_Stuff ();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKeyDown("space")) //change the turn
		{
			turn = !turn;
		}
		if (Input.GetMouseButtonDown(0) && turn) //if its your turn and you click
		{
			Vector2 move = new Vector2();
			move = Culll(Camera.main.ScreenToWorldPoint(new Vector2 (Input.mousePosition.x, Input.mousePosition.y)));  //move is the square where you clicked
			if (on_board(move) && Validate(move)) //if that square is empty
			{
				ref_board[(int)white_pos.x, (int)white_pos.y] = 2; //the old space is made empty
				ref_board[(int)move.x, (int)move.y] = 1;  //your move is your new position
				white_pos = new Vector2(move.x, move.y);  //saves your move for later reference
				Place_Stuff();
			}
		}
	}

	public bool on_board(Vector2 move) //makes sure the move is valid
	{
		if (move.x >= 0 && move.x <= 7 && move.y >= 0 && move.y <= 7)
			return true;
		else
			return false;
	}
	public bool on_board(int movex, int movey) //same thing
	{
		if (movex >= 0 && movex <= 7 && movey >= 0 && movey <= 7)
			return true;
		else
			return false;
	}
	public Vector3 Culll(Vector2 mov) //makes the spot clicked align with the grid.
	{
		return new Vector2 (Mathf.Floor (mov.x + .5f), Mathf.Floor (mov.y + .5f)); 
	}
	public void Place_Stuff()  //use this after a valid move has been made, and the matrix updated
	{
		for (int i = 0; i <8; i++) 
		{
			for(int j = 0; j < 8; j++)
			{
				if (ref_board[i,j] != 0)
				{
					Destroy(real_board[i,j]);
					if (ref_board[i,j] == 2)
						ref_board[i,j] = 0;
				}
				if (ref_board[i,j] == 1)
				{
					real_board[i,j] = Instantiate(white1, new Vector3(i,j,11), Quaternion.identity);
				}
				else if (ref_board[i,j] == -1)
				{
					real_board[i,j] = Instantiate(black1, new Vector3(i,j,11), Quaternion.identity);
				}
			}
		}
	}

	public bool Validate(Vector2 mov)
	{
		if (ref_board[(int)mov.x, (int)mov.y] == 0) 
		{
			if (mov.x == white_pos.x + 1 && mov.y == white_pos.y)
				return true;
			else if (mov.x == white_pos.x - 1 && mov.y == white_pos.y)
				return true;
			else if (mov.x == white_pos.x && mov.y == white_pos.y + 1)
				return true;
			else if (mov.x == white_pos.x && mov.y == white_pos.y - 1)
				return true;
			else
				return false;
		}
		else
			return false;
	}
}
