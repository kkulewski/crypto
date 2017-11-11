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
