#pragma once
namespace std_vector
{
    template<typename T>
    class vector
    {
    private:
        T* _vBase;
        size_t _vCapacity;
        size_t _vSize;

        size_t getCapacityForIncrease()
        {
            if (_vCapacity > 0)
            {
                return _vCapacity *= 2;
            }
            return _vCapacity = 1;
        }

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
                --pos;
                return (*this);
            }
            const reverseIterator operator++(int)
            {
                const reverseIterator rtn(*this);
                --pos;
                return rtn;
            }
            reverseIterator &operator--()
            {
                ++pos;
                return (*this);
            }
            const reverseIterator operator--(int)
            {
                const reverseIterator rtn(*this);
                ++pos;
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
                ++pos;
                return (*this);
            }
            const iterator operator++(int)
            {
                const iterator rtn(*this);
                ++pos;
                return rtn;
            }
            iterator &operator--()
            {
                --pos;
                return (*this);
            }
            const iterator operator--(int)
            {
                const iterator rtn(*this);
                --pos;
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
            _vBase = nullptr;
            _vCapacity = 0;
            _vSize = 0;
        }
        vector(const vector& vt)
            : _vBase(new T[vt._vCapacity]), _vCapacity(vt._vCapacity), _vSize(vt._vSize)
        {
            for (size_t index = 0; index < vt._vSize; ++index)
            {
                _vBase[index] = vt._vBase[index];
            }

            std::cout << "copy_constructor" << endl;
        }
        vector(vector&& vt) noexcept
            : _vBase(vt._vBase), _vCapacity(vt._vCapacity), _vSize(vt._vSize)
        {
            vt._vBase = nullptr;
            vt._vCapacity = 0;
            vt._vSize = 0;
            std::cout << "move_constructor" << endl;
        }
        virtual ~vector()
        {
            if (_vBase != nullptr)
            {
                delete[] _vBase;
            }
        }
        T& operator[] (size_t index) const
        {
            if ((index >= 0) && (index < _vSize))
            {
                return _vBase[index];
            }
            // exeception
        }
        void resize(size_t nsize, T data = 0)
        {
            if (nsize > _vCapacity)
            {
                reserve(getCapacityForIncrease());
            }
            while (_vSize < nsize)
            {
                push_back(data);
            }
        }
        void reserve(size_t nCapacity)
        {
            T* temp = new T[nCapacity];
            if (_vSize)
            {
                for (size_t index = 0; index < _vSize; ++index)
                {
                    temp[index] = std::move(_vBase[index]);
                }
                delete[] _vBase;
            }
            _vBase = temp;

            _vCapacity = nCapacity;
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
            size_t index = at - _vBase;

            if (_vSize >= _vCapacity)
            {
                reserve(getCapacityForIncrease());
            }

            for (size_t pos = _vSize; pos > index; pos--)
            {
                _vBase[pos] = std::move(_vBase[pos - 1]);
            }
            _vBase[index] = data;
            ++_vSize;
        }
        void erase(iterator at)
        {
            --_vSize;
            for (size_t index = at - _vBase; index < _vSize; ++index)
            {
                _vBase[index] = std::move(_vBase[index + 1]);
            }
        }
        iterator begin() const
        {
            iterator iter(_vBase);
            return iter;
        }
        iterator end() const
        {
            iterator iter(_vBase + _vSize);
            return iter;
        }
        reverseIterator rbegin() const
        {
            reverseIterator iter(_vBase + _vSize - 1);
            return iter;
        }
        reverseIterator rend() const
        {
            reverseIterator iter(_vBase - 1);
            return iter;
        }
        const iterator cbegin() const
        {
            iterator iter(_vBase);
            return iter;
        }
        const iterator cend() const
        {
            iterator iter(_vBase + _vSize);
            return iter;
        }
        size_t size() const
        {
            return _vSize;
        }
        size_t capacity() const
        {
            return _vCapacity;
        }
        bool empty() const
        {
            return _vSize == 0;
        }
        void shrink_to_fit()
        {
            _vCapacity = _vSize;
        }
        T at(size_t index) const
        {
            if (index >= 0 && index < _vSize)
            {
                return _vBase[index];
            }
            // exeception
        }
        T front() const
        {
            return *_vBase;
        }
        T back() const
        {
            return _vBase[_vSize - 1]
        }
        T* data() const
        {
            return _vBase;
        }
    };
};