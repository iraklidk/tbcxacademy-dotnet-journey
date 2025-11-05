namespace Practice
{
    internal class CountryMustHaveSingleCapitalException : Exception
    {
        public CountryMustHaveSingleCapitalException()
            : base("A country must have exactly one capital city.") { }
    }
}
