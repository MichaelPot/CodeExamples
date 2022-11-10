package build;

import javax.crypto.KeyGenerator;
import javax.crypto.spec.SecretKeySpec;

import java.io.Serializable;
import java.security.Key;
import java.security.NoSuchAlgorithmException;
import java.security.SecureRandom;

import org.apache.commons.codec.binary.Hex;

public class SecretKey implements Serializable {

    protected Key key;

    public SecretKey(String encodedKey) {
        try {
            byte[] decodedKey = Hex.decodeHex(encodedKey);
            key = new SecretKeySpec(decodedKey, 0, decodedKey.length, "AES"); 
        } catch(Exception e) {
            Gradebook.exit("Decode key");
        }
    }

    public SecretKey() throws NoSuchAlgorithmException {
        KeyGenerator keyGen = KeyGenerator.getInstance("AES");
        SecureRandom sRandom = new SecureRandom();

        keyGen.init(sRandom);
        key = keyGen.generateKey();
    }

    public String toString() {
        return Hex.encodeHexString(key.getEncoded());
    }

    public boolean equals(Object obj) {
        if (this == obj) return true;
        if (obj == null || getClass() != obj.getClass())
            return false;
        SecretKey sKey = (SecretKey) obj;
        return key.equals(sKey.key);
    }
}