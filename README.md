# Cryptography 2017/2018 at MFI, University of Gdansk

## 1. Caesar & Affine (rotational cipher)
#### Prerequisites
- plain.txt - source text
- key.txt - key
- extra.txt - helper text for cryptoanalysis (part of source text)
#### Usage
- AffineCipher.exe -[a|c] -[e|d|k|j]
- arg1: a (affine mode), c (caesar mode)
- arg2: e (encrypt), d (decrypt), j (cryptoanalysis with helper text), k (brute force)
#### Remarks
- all characters except for english letters are ignored
- key has format Ax + B, A is ignored in caesar mode


## 2. OTP / One-Time-Password (xor cipher)
#### Prerequisites
- oring.txt - source text
- key.txt - key
#### Usage
- OTP.exe -[p|e|k]
- arg1: p (prepare text), e (encrypt), k (brute force)
#### Remarks
- line length = 16
- CR/LF is not taken into account


## 3. ECB & CBC (block cipher)
#### Prerequisites
- plain.bmp - image with 24 bits per pixel (bitmap-24)
- key.txt - key (binary number)
#### Usage
- Block.exe
#### Remarks
- block size is determined by BlockWidth and BlockHeight consts
- bitmap width = multiple of BlockWidth, height respectively
- key length has to be greater or equal to block size (BlockWidth * BlockHeight)
- default block size: 4x4 px, key = 16


## 4. Hash functions
#### Prerequisites
- hash.txt - file with computed hash-pairs
#### Usage
- diff.rb > diff.txt
#### Remarks
- hash pair contains 2 function calls followed by 2 computed hashes (4 successive lines)
- summary takes two args - entry line-start index and source file 


## 5. Vigenere
#### Prerequisites
- oring.txt - source text
- key.txt - key
#### Usage
- Vigenere.exe -[p|e|d|k]
- arg1: p (prepare text), e (encrypt), d (decrypt), k (brute force)
#### Remarks
- only english texts are valid because of defined letter frequencies


## 6. ElGamal
#### Prerequisites
- plain.text - source text for encryption
- message.txt - source text for signature
- elgamal.txt - generator and prime number
#### Usage
- ElGamal.exe -[k|e|d|s|v]
- arg1: k (generate keys), e (encrypt), d (decrypt), s (sign), v (verify signature)
#### Remarks
- source text consists of a single number (interpreted as a number, not ASCII)
