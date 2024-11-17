namespace JO.Data.Base.ValueObjects
{
    public abstract class ValueObject<VO>
        where VO : ValueObject<VO>
    {
        protected static bool EqualOperator(ValueObject<VO> left, ValueObject<VO> right)
        {
            if (ReferenceEquals(left, null) ^ ReferenceEquals(right, null))
            {
                return false;
            }
            return ReferenceEquals(left, right) || left.Equals(right);
        }

        protected static bool NotEqualOperator(ValueObject<VO> left, ValueObject<VO> right)
        {
            return !(EqualOperator(left, right));
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }

            var other = (ValueObject<VO>)obj;

            return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public static bool operator == (ValueObject<VO> one, ValueObject<VO> two)
        {
            return EqualOperator(one, two);
        }

        public static bool operator != (ValueObject<VO> one, ValueObject<VO> two)
        {
            return NotEqualOperator(one, two);
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }

        public abstract void Validate();

        protected abstract void Update(VO other);
        protected abstract void Update(object parameter);
        protected abstract void Update(object parameter, object parameter2);
        protected abstract void Update(object parameter, object parameter2, object parameter3);
    }
}
