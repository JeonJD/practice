#include "consumer.h"

Consumer::Consumer(int consumerIndex, std::mutex& mx, std::condition_variable& cb, bool& finished, std::queue<int>& itemQueue)
    :_consumerIndex(consumerIndex), _mutex(mx), _conditionVariable(cb), _finished(finished), _itemQueue(itemQueue)
{
}

Consumer::~Consumer()
{
}

void Consumer::ConsumeItem()
{
    do
    {
        {
            std::unique_lock<std::mutex> lk(_mutex);
            _conditionVariable.wait(lk, [&] { return _finished || !_itemQueue.empty(); });

            if (!_itemQueue.empty())
            {
                std::cout << "consuming(" << _consumerIndex << "): " << _itemQueue.front() << std::endl;
                _itemQueue.pop();
            }
        }
        {
            std::this_thread::sleep_for(std::chrono::milliseconds(1));
        }
    } while (!(_finished && _itemQueue.empty()));
}