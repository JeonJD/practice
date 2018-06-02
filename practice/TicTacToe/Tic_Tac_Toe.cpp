/*
https://codegolf.stackexchange.com/questions/144116/tic-tac-toe-x-or-o/144122
Task

Given a Tic-Tac-Toe board at the end of a game
(in the form of a string, a matrix,
a flat list of 9 ordered values, any other decent format),
determine who wins the game.

The input will consist of distinct and consistent values,
one for X, one for O and another one that represents an empty spot.

Your program should be able to output 3 distinct,
consistent and non-empty values: one in case X wins,
another one in case O wins or another if the players are tied.

Please specify these values in your answer.
You can assume that the input will be a valid Tic-Tac-Toe board.

Test Cases

X, O, _ are the input values here; X wins, O wins and Tie are for the output.

X O X
O X _
O _ X

Output: X wins.

X _ O
X O _
X O X

Output: X wins.

X O X
_ O X
_ O _

Output: O wins.

X O X
O O X
X X O

Output: Tie.
*/
#include <iostream>

#define SIZE 3

using namespace::std;

int getIndexFromMatrix(int x, int y)
{
    return x + y * SIZE;
}

bool checkBingo(char* line)
{
    if (line == nullptr) return false;

    for (size_t i = 0; i < (SIZE - 1); ++i)
    {
        if (*line == '_') return false;
        if (*(line + i) != *(line + i + 1)) return false;
    }

    cout << *line << " wins." << endl;

    return true;
}

void main()
{
    while (1)
    {
        bool gameover(false);
        char board[SIZE * SIZE + 1];
        for (auto c : board) c = '\0';
        cin >> board;

        char dia[2][SIZE];

        for (size_t i = 0; i < SIZE; ++i)
        {
            dia[0][i] = board[getIndexFromMatrix(i, i)]; //우상to좌하 대각선 열 저장
            dia[1][i] = board[getIndexFromMatrix(i, (SIZE - 1) - i)]; //좌상to우하 대각선 저장

            char row[SIZE];
            char cul[SIZE];

            for (size_t j = 0; j < SIZE; ++j)
            {
                row[j] = board[getIndexFromMatrix(i, j)]; //가로 저장
                cul[j] = board[getIndexFromMatrix(j, i)]; //세로 저장
            }
            if (gameover || checkBingo(row) || checkBingo(cul)) { gameover = true; break; }
            //가로 세로 한열씩 검사하여 승리 발생시 바로 루프 종료
        }

        if (gameover || checkBingo(dia[0]) || checkBingo(dia[1])) continue; //대각선 검사

        cout << "Tie." << endl;
    }
}