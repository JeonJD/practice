#include "main.h"

void main()
{
    std::mutex mutex;
    std::condition_variable conditionVariable;
    std::queue<int> itemQueue;
    std::vector<std::thread*> trdConsumers;
    bool finished = false;

    Producer producer(ITEM_COUNT, QUEUE_SIZE, mutex, conditionVariable, itemQueue);
    std::thread trdProducer(&Producer::ProduceItem, producer);

    for (int i = 0; i < CONSUMER_COUNT; ++i)
    {
        Consumer consumer(i, mutex, conditionVariable, finished, itemQueue);
        std::thread* trdConsumer = new std::thread(&Consumer::ConsumeItem, consumer);
        trdConsumers.push_back(trdConsumer);
    }

    trdProducer.join();
    finished = true;

    for (std::thread* consumer : trdConsumers)
    {
        if (consumer)
        {
            consumer->join();
            delete consumer;
        }
    }

    std::cout << "finish" << std::endl;
    system("pause");
}