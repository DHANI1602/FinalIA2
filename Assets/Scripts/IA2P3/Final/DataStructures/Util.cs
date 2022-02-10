using System;
using System.Collections;
using System.Collections.Generic;

public static class Util {

    public static IEnumerable<T> Generate<T>(T seed, Func<T, T> generator) {
        var acum = seed;
        while (true) {
            yield return acum;
            acum = generator(acum);
        }
    }
    
}
        