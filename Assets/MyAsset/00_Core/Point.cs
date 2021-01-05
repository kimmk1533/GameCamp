[System.Serializable]

//포인트 구조체.
//(사용하던 구조체가 int로 설정해 int 기준으로 만든거라 float로 변경해도 되긴하지만 변경 시 일부 함수 사용 불가).

public struct Point
{
    public int x;
    public int y;
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }   //생성자(xy 변수 값 대입).

    //연산자 중복(재정의) 사용.
    public static Point operator +(Point a, Point b)
    {
        Point temp;
        temp.x = a.x + b.x;
        temp.y = a.y + b.y;
        return temp;
    }
    public static Point operator -(Point a, Point b)
    {
        Point temp;
        temp.x = a.x - b.x;
        temp.y = a.y - b.y;
        return temp;
    }
    public static bool operator ==(Point a, Point b)
    {
        return (a.x == b.x && a.y == b.y);
    }
    public static bool operator !=(Point a, Point b)
    {
        return !(a == b);
    }

    /*---------------------------------------------------------------------------------------
     * Equals와 GetHashCode 오버라이딩.
    (배열 안에 포함 되어있는지 확인하는 Contains()를 사용하면 오브젝트 박싱으로 인한 가비지가 생겨서 대체).*/
    public override bool Equals(object obj)
    {
        if (obj is Point)
        {
            Point p = (Point)obj;
            return (x == p.x && y == p.y);
        }
        return false;
    }
    public bool Equals(Point p)
    {
        return (x == p.x && y == p.y);
    }

    public override int GetHashCode()
    {
        return x ^ y;   //둘 다 false일 시 true인 연산자.
    }

    //{0}의 값에 x좌표를 {1}에 y좌표를 넣어, (x, y) 형대로 문자열을 만들어서 반환한다.
    public override string ToString()
    {
        return string.Format("{0},{1}", x, y);
    }
    /*---------------------------------------------------------------------------------------*/
}