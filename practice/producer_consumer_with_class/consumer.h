#pragma once

#include <iostream>
#include <queue>
#include <mutex>
#include <chrono>

class Consumer
{
public:
    Consumer(int consumerIndex, std::mutex& mx, std::condition_variable& cb, bool& finished, std::queue<int>& itemQueue);
    ~Consumer();

    void ConsumeItem();

private:
    int _consumerIndex;
    std::mutex& _mutex;
    std::condition_variable& _conditionVariable;
    bool& _finished;
    std::queue<int>& _itemQueue;
};