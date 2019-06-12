/* Adapted to C# from a minimax function in javascript
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Application
{

    public class Data
    {
        public int x;
        public int y;
        public int score;
        public Data(int x, int y, int score)
        {
            this.x = x;
            this.y = y;
            this.score = score;
        }

    };

    public class Cell
    {
        public int x;
        public int y;
        public Cell(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    };

    public class AITicTacToe : System.Object
    {

        public int PLAYER = -1;
        public int OTHERPLAYER = 1;

        public void reset()
        {
        }

        int evaluate(int[,] state)
        {
            int score = 0;

            if (gameOver(state, OTHERPLAYER))
            {
                score = 1;
            }
            else if (gameOver(state, PLAYER))
            {
                score = -1;
            }

            return score;
        }

        bool gameOver(int[,] state, int player)
        {
            int[,] win_state = {
            {state[0,0], state[0,1], state[0,2]},
            {state[1,0], state[1,1], state[1,2]},
            {state[2,0], state[2,1], state[2,2]},
            {state[0,0], state[1,0], state[2,0]},
            {state[0,1], state[1,1], state[2,1]},
            {state[0,2], state[1,2], state[2,2]},
            {state[0,0], state[1,1], state[2,2]},
            {state[2,0], state[1,1], state[0,2]}
        };
            for (int i = 0; i < 8; i++)
            {
                int line0 = win_state[i, 0];
                int line1 = win_state[i, 1];
                int line2 = win_state[i, 2];
                var filled = 0;
                if (line0 == player && line1 == player && line2 == player)
                {
                    return true;
                }

            }
            return false;
        }

        bool gameOverAll(int[,] state)
        {
            return gameOver(state, PLAYER) || gameOver(state, OTHERPLAYER);
        }

        List<Cell> emptyCells(int[,] state)
        {
            List<Cell> cells = new List<Cell>();
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    if (state[x, y] == 0)
                    {
                        cells.Add(new Cell(x, y));
                    }
                }
            }
            return cells;
        }

        bool validMove(int x, int y, int[,] board)
        {

            if (board[x, y] == 0) { return true; }
            return false;
        }

        string boardState(int[,] state)
        {
            string s = "";
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    var v = state[x, y];
                    s += v;
                }
            }
            return s;
        }


        public int[,] setMove(int x, int y, int[,] board, int player)
        {
            if (validMove(x, y, board))
            {
                //Debug.Log("initial state: " + boardState(board));
                board[x, y] = player;
                string s = boardState(board);
                //Debug.Log("state: " + s);
                return board;
            }
            return board;
        }

        public Data minimax(int[,] state, int depth, int player)
        {
            Data best = null;
            if (player == OTHERPLAYER)
            {
                best = new Data(-1, -1, -1000);
            }
            else
            {
                best = new Data(-1, -1, +1000);
            }
            if (depth == 0 || gameOverAll(state))
            {

                int score = evaluate(state);
                return new Data(-1, -1, score);
            }

            var cells = emptyCells(state);
            cells.ForEach((Cell obj) =>
            {
                int x = obj.x;
                int y = obj.y;
                state[x, y] = player;
                var score = minimax(state, depth - 1, -player);
                state[x, y] = 0;
                score.x = x;
                score.y = y;

                if (player == OTHERPLAYER)
                {
                    if (score.score > best.score)
                    {
                        best = score;
                    }
                }
                else
                {
                    if (score.score < best.score)
                    {
                        best = score;
                    }
                }
            });
            return best;
        }

        int getDifference(int[,] board)
        {
            return 0;
        }

        //update board
        public int[,] aiMove(int[,] board, int playerid)
        {
            int x, y = 0;
            Data move;
            if (emptyCells(board).Count == 9)
            {
                x = 1;
                y = 1;
            }
            else
            {
                move = minimax(board, emptyCells(board).Count, playerid);
                x = move.x;
                y = move.y;
                Debug.Log(x + " - " + y + " " + boardState(board));
            }


            board = setMove(x, y, board, playerid);
            if (gameOverAll(board))
            {
                Debug.Log("ENDGAME WINNER");
            }
            else if (emptyCells(board).Count == 0)
            {
                Debug.Log("ENDGAME DRAW");
            }
            return board;
        }


    }
}
