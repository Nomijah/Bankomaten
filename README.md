# Bankomaten
A school assignment to create a virtual ATM machine/banking app without using classes for users and accounts.

There are five users available in the program who each has their own pin number. User info and accounts are stored in the class GlobalInfo where there's also an array
with names of account types.
The basic structure of the program is that when a user logs in, his/her accounts are retrieved from the GlobalInfo class and saved in a
decimal array which then is sent in and out of the different methods to display or change the information in them. Every even index in the account array holds an index
number to retrieve the account name and every other holds the balance of that account, so each account takes up two places in the array.
When the user logs out the information in the account array overwrites the original array in GlobalInfo so that the correct balance of the accounts is saved. I made the
GlobalInfo class to simulate a database that can be read from and written to.

Given the restrictions of how we were allowed to make this program I feel that I have found a good solution to the given tasks. I have taken measures to ensure that the
program doesn't crash in most cases, I know of a few things I could add to improve that even further but I think that it's unlikely that anyone would encounter any 
problems. Given the structure of the program one can easily add more functions without altering the existing code and I think that it is obvious what each method does
and it's easy to follow along in the code. I'm happy with the result and the only way I would be more happy with it is if I were allowed to remake it with object classes
instead of arrays for the user info and accounts.
