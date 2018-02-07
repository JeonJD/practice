#pragma once

#include <iostream>
#include <queue>
#include <mutex>

class Producer
{
public:
    Producer(int iCount, int qSize, std::mutex& mx, std::condition_variable& _conditionVariable, std::queue<int>& itemQueue);
    ~Producer();

    bool ProduceItem();

private:
    int _itemCount;
    int _queue_size;
    std::mutex& _mutex;
    std::condition_variable& _conditionVariable;
    std::queue<int>& _itemQueue;
};