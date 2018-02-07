#include "producer.h"

Producer::Producer(int iCount, int qSize, std::mutex& mx, std::condition_variable& cb, std::queue<int>& itemQueue)
    :_itemCount(iCount), _queue_size(qSize), _mutex(mx), _conditionVariable(cb), _itemQueue(itemQueue)
{
}

Producer::~Producer()
{
}

bool Producer::ProduceItem()
{
    int i = 0;

    while (i < _itemCount)
    {
        if (static_cast<int>(_itemQueue.size()) < _queue_size)
        {
            std::lock_guard<std::mutex> lk(_mutex);
            _itemQueue.push(i);
            std::cout << "producing: " << i << " currunt queue(" << _itemQueue.size() << "/" << _queue_size << ")" << std::endl;
            ++i;
        }
        _conditionVariable.notify_all();
    }

    return true;
}