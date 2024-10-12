using Sqids;

namespace CommonTestUtilities.IdEncrypt;

public class IdEncryptBuilder
{
    public static SqidsEncoder<long> Builder()
    {
        return new SqidsEncoder<long>(new()
        {
            MinLength = 3,
            Alphabet = "d1SMPg2YaQDEpuFOVRBU845soG0qnLmArefly3N"
        });
    }
}