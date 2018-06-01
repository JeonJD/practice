namespace std_vector
{
    template<typename T>
    class vector
    {
    private:
        T *base;
        size_t bcapacity;
        size_t bsize;

    public:
        class iterator
        {
        private:
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
            iterator &operator++()
            {
                pos++;
                return (*this);
            }
            const iterator operator++(int)
            {
                iterator rtn(*this);
                pos++;
                return rtn;
            }
            bool operator != (const iterator &iter) const
            {
                return pos != iter.pos;
            }
            bool operator == (const iterator &iter) const
            {
                return pos == iter.pos;
            }
        };
        typedef iterator const_iterator;
        vector()
        {
            base = 0;
            bcapacity = bsize = 0;
        }
        ~vector()
        {
            if (base)
            {
                delete[] base;
            }
        }
        void resize(size_t nsize, T data = 0)
        {
            if (nsize > bcapacity)
            {
                reserve(nsize);
            }
            while (bsize < nsize)
            {
                push_back(data);
            }
        }
        void reserve(size_t ncapacity)
        {
            T *temp = new T[ncapacity];
            bcapacity = ncapacity;
            if (bsize)
            {
                for (size_t n = 0; n < bsize; n++)
                {
                    temp[n] = base[n];
                }
                delete[] base;
            }
            base = temp;
        }
        void push_back(T data)
        {
            insert(end(), data);
        }
        void insert(iterator at, T data)
        {
            size_t index = at - base;

            if (bsize >= bcapacity)
            {
                reserve(bcapacity * 2);
            }

            for (size_t pos = bsize; pos > index; pos--)
            {
                base[pos] = base[pos - 1];
            }
            base[index] = data;
            bsize++;
        }
        void erase(iterator at)
        {
            bsize--;
            for (size_t index = at - base; index < bsize; index++)
            {
                base[index] = base[index + 1];
            }
        }
        T &operator[] (size_t index) const
        {
            if ((index >= 0) && (index < bsize))
            {
                return base[index];
            }
        }
        iterator begin() const
        {
            iterator iter(base);
            return iter;
        }
        iterator end() const
        {
            iterator iter(base + bsize);
            return iter;
        }
        size_t size()
        {
            return bsize;
        }
        size_t capacity()
        {
            return bcapacity;
        }
    };
};
void main()
{
}