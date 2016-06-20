#ifndef SNAP7CLIENT_H
#define SNAP7CLIENT_H
#define NUM_THREADS 5


class snap7Client
{
    public:
        snap7Client();
        virtual ~snap7Client();
        void Usage();
        int ConnectTo();

    protected:

    private:
};

#endif // SNAP7CLIENT_H
