#include <iostream>
#include "std_vector.h"
using namespace std;
using std_vector::vector;

void main()
{
    vector<int> vt;
    for (size_t i = 0; i < 10; ++i)
    {
        vt.push_back(i);
    }

    for (size_t i = 0; i < vt.size(); ++i)
    {
        cout << vt[i] << endl;
    }
    system("pause");
    return;
}