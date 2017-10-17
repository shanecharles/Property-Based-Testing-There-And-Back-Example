#I "packages/FsCheck/lib/net452/"
#r "FsCheck.dll"

#I "ThereAndBack/bin/debug/"
#r "ThereAndBack.dll"

open FsCheck
open System

let encryption = ThereAndBack.Encryption ()

let ``Check there and back of DateTime encryption`` (input : DateTime) =
  input = (input.ToString() 
            |> encryption.EncryptStringToBytes_Aes 
            |> encryption.DecryptStringFromBytes_Aes 
            |> DateTime.Parse)

let ``Check there and back for non empty string encryption`` (NonEmptyString input) =
  input = (input 
            |> encryption.EncryptStringToBytes_Aes 
            |> encryption.DecryptStringFromBytes_Aes)

Check.Quick ``Check there and back of DateTime encryption``
Check.Quick ``Check there and back for non empty string encryption``