#include <thread>
#include <iostream>
#include <queue>
#include <mutex>
#include <chrono>

#define QUEUE_SIZE 10
#define ITEM_COUNT 100

std::mutex _mutex;
std::condition_variable _conditionVariable;
std::queue<int> _itemQueue;
bool _finished = false;

void Producer(int n)
{
    int i = 0;

    while (i < ITEM_COUNT)
    {
        if (_itemQueue.size() < QUEUE_SIZE)
        {
            std::lock_guard<std::mutex> lk(_mutex);
            _itemQueue.push(i++);
            std::cout << "producing: " << i << std::endl;
        }
        else
        {
            continue;
        }
        _conditionVariable.notify_all();
    }

    std::lock_guard<std::mutex> lk(_mutex);
    _finished = true;
    _conditionVariable.notify_all();
}

void Consumer(int index)
{
    while (!(_finished && _itemQueue.empty()))
    {
        std::this_thread::sleep_for(std::chrono::seconds(1));
        std::unique_lock<std::mutex> lk(_mutex);
        _conditionVariable.wait(lk, [] { return _finished || !_itemQueue.empty(); });
        if (!_itemQueue.empty())
        {
            std::cout << "consuming(" << index << "): " << _itemQueue.front() << std::endl;
            _itemQueue.pop();
        }
    }
}

void main()
{
    std::thread trdProducer(Producer, ITEM_COUNT);
    std::thread trdConsumer1(Consumer, 1);
    std::thread trdConsumer2(Consumer, 2);
    std::thread trdConsumer3(Consumer, 3);

    trdProducer.join();
    trdConsumer1.join();
    trdConsumer2.join();
    trdConsumer3.join();

    std::cout << "finish" << std::endl;
    system("pause");
}