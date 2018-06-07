#pragma once
namespace std_vector
{
    template<typename T>
    class vector
    {
    private:
        T* base;
        size_t vCapacity;
        size_t vSize;

    public:
        class reverseIterator : public iterator
        {
        public:
            reverseIterator(T *pos = 0)
            {
                this->pos = pos;
            }
            int operator- (const reverseIterator &iter) const
            {
                return -(pos - iter.pos);
            }
            reverseIterator operator+ (const int index) const
            {
                return pos - index;
            }
            reverseIterator operator- (const int index) const
            {
                return pos + index;
            }
            reverseIterator &operator++()
            {
                pos--;
                return (*this);
            }
            const reverseIterator operator++(int)
            {
                const reverseIterator rtn(*this);
                pos--;
                return rtn;
            }
            reverseIterator &operator--()
            {
                pos++;
                return (*this);
            }
            const reverseIterator operator--(int)
            {
                const reverseIterator rtn(*this);
                pos++;
                return rtn;
            }
        };

        class iterator
        {
        protected:
            T *pos;

        public:
            iterator(T *pos = 0)
            {
                this->pos = pos;
            }
            T operator *() const
            {
                return *pos;
            }
            int operator- (const iterator &iter) const
            {
                return pos - iter.pos;
            }
            iterator operator+ (const int index) const
            {
                return pos + index;
            }
            iterator operator- (const int index) const
            {
                return pos - index;
            }
            iterator &operator++()
            {
                pos++;
                return (*this);
            }
            const iterator operator++(int)
            {
                const iterator rtn(*this);
                pos++;
                return rtn;
            }
            iterator &operator--()
            {
                pos--;
                return (*this);
            }
            const iterator operator--(int)
            {
                const iterator rtn(*this);
                pos--;
                return rtn;
            }
            bool operator!= (const iterator &iter) const
            {
                return pos != iter.pos;
            }
            bool operator== (const iterator &iter) const
            {
                return pos == iter.pos;
            }
            bool operator!= (const reverseIterator &iter) const
            {
                return pos != iter.pos;
            }
            bool operator== (const reverseIterator &iter) const
            {
                return pos == iter.pos;
            }
        };

        vector()
        {
            base = nullptr;
            vCapacity = 0;
            vSize = 0;
        }
        vector(const vector& vt)
        {
            base = new T[vt.vCapacity];
            for (size_t index = 0; index < vt.vSize; ++index)
            {
                base[index] = vt.base[index];
            }
            vCapacity = vt.vCapacity;
            vSize = vt.vSize;
            std::cout << "copy" << endl;
        }
        vector(vector&& vt)
        {
            base = vt.base;
            vCapacity = vt.vCapacity;
            vSize = vt.vSize;

            vt.base = nullptr;
            vt.vCapacity = 0;
            vSize = 0;
            std::cout << "move" << endl;
        }

        ~vector()
        {
            if (base != nullptr)
            {
                delete[] base;
            }
        }
        void resize(size_t nsize, T data = 0)
        {
            if (nsize > vCapacity)
            {
                reserve(nsize);
            }
            while (vSize < nsize)
            {
                push_back(data);
            }
        }
        void reserve(size_t nCapacity)
        {
            T* temp = new T[nCapacity];
            if (vSize)
            {
                for (size_t n = 0; n < vSize; n++)
                {
                    temp[n] = base[n];
                }
                delete[] base;
            }
            base = temp;

            vCapacity = nCapacity;
        }
        void push_back(T data)
        {
            insert(end(), data);
        }
        void insert(reverseIterator at, T data)
        {
            insert(iterator(at) + 1, data);
        }
        void insert(iterator at, T data)
        {
            size_t index = at - base;

            if (vSize >= vCapacity)
            {
                size_t nCapacity = 1;
                if (vCapacity)
                {
                    nCapacity = vCapacity * 2;
                }
                reserve(nCapacity);
            }

            for (size_t pos = vSize; pos > index; pos--)
            {
                base[pos] = base[pos - 1];
            }
            base[index] = data;
            vSize++;
        }
        void erase(iterator at)
        {
            vSize--;
            for (size_t index = at - base; index < vSize; index++)
            {
                base[index] = base[index + 1];
            }
        }
        T &operator[] (size_t index) const
        {
            if ((index >= 0) && (index < vSize))
            {
                return base[index];
            }
            // exeception
        }
        T &operator= (T) const
        {
            if ((index >= 0) && (index < vSize))
            {
                return base[index];
            }
            // exeception
        }
        iterator begin() const
        {
            iterator iter(base);
            return iter;
        }
        iterator end() const
        {
            iterator iter(base + vSize);
            return iter;
        }
        reverseIterator rbegin() const
        {
            reverseIterator iter(base + vSize - 1);
            return iter;
        }
        reverseIterator rend() const
        {
            reverseIterator iter(base - 1);
            return iter;
        }
        const iterator cbegin() const
        {
            iterator iter(base);
            return iter;
        }
        const iterator cend() const
        {
            iterator iter(base + vSize);
            return iter;
        }
        size_t size() const
        {
            return vSize;
        }
        size_t capacity() const
        {
            return vCapacity;
        }
        bool empty() const
        {
            return vSize == 0;
        }
        void shrink_to_fit()
        {
            vCapacity = vSize;
        }
        T at(size_t index) const
        {
            if (index >= 0 && index < vSize)
            {
                return base[index];
            }
            // exeception
        }
        T front() const
        {
            return *base;
        }
        T back() const
        {
            return base[vSize - 1]
        }
        T* data() const
        {
            return base;
        }
    };
};