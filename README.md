###Tweet Stats

**Requirements**<br/>
You will need to install the mono runtime</br>
http://www.mono-project.com/docs/advanced/runtime/

You may need to download the compiler and compile the solution on the target machine</br>
http://www.mono-project.com/download/#download-mac

**Usage**</br>
run.sh

**Optional Arguments**</br>
-h     to display the help text</br>
-ds x  to choose a data structure, where x is 0, 1, or 2

* 0 = hashtable &nbsp;&nbsp;(default if argument or value is not supplied)
* 1 = trie
* 2 = ternary tree


**Why different data structures?**</br>
When I first started this project, I started working on the file reading process. I tried a few different ways and tested how fast each one worked. 

&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;**3 file reading methods:**
- Read the file to the end, put it in a buffer, and process the data.  This would not scale.  If I try to read the whole file into memory first, with a large enough file, I would run out of memory.

- Read the file line by line. This is a nice option, since I wouldn't have to parse the file for new lines, it is already being
done for me.

- Read the file a block at a time. This requires a little bit more coding in order to parse and look for new lines. However, this was the fastest option, most likely because there are less round trips to the file and back. I played around with different buffer sizes, and it didn't really have any effect on performance.  I finally left it at 8KB.

After realizing that there's only so much I can do to speed up the file reading process, I then started focusing on other components that could affect the performance of this process.  This is where I started thinking of which data structures I could use to store all of the unique words and their count of occurrences. 

</br>
### Data Structures
* **Hashtable**</br>
This uses the native .NET Dictionary which is implemented as a hash table underneathe.  This is the easiest solution to implement. It's just a hashtable with the key being the word, and the value is the occurrences of that word. You keep adding words to the Dictionary, along with its count, as you parse the file. At the end, you need to sort the keys, and output the unique words. So far, from the tests i've done, it runs the fastest. My only concern is how scalable is it. Given a month's worth of tweets, if I store the words on 1 hashtable on 1 computer, it would definitely run out of memory. This is where a distrubuted hash table would come in handy. If you could take advantage of the memory on multiple computers, this should work.
</br></br>Since, we are running this process on 1 machine, I felt that some kind of persistant storage would be necessary.  I didn't have much time to spend on this part. I looked into a few fast, lightweight key/value stores for .NET. I thought, this will definitely slow down the performance, but at least it would be scalable. However, here is the problem I ran into. One of the key/value stores, BinaryRage, didn't have any way to retrieve the whole collection. You could only retrieve 1 element at time.
How then could I sort the data?  So that wasn't going to work.  I found another key/value store called RaptorDB, which did have
a way to get all elements, and enumerate through them. The problem was, you could only get the full collection, not a subset. This means that I'm going back to my original problem of scalability, since I would have to load the full collection into memory, sort it, and then output it. 
</br></br>My solution to this, which I didn't get to implement, was SQLite, or maybe a NoSQL db. I would build a table with the columns (word, count), and just store all my words in there.  The advantage I would have when I want to output the words to a file, is that I could then query the db for only a chunk of words at a time, and then write them to the file, for example all characters starting with ASCII code 0 to 50, then 51 to 100, and so on.  I could use the query to sort the data, and not do it in .NET.  I am not sure which would be faster.  But storing the words in the db, instead of keeping it memory, and querying the data chunks at a time, instead of loading the full collection into memory, would give me the scalability I was looking for.</br></br>


* **Trie**</br>
The reason I thought of this data structure is because I figured that with so many words, there are going to many words with the same prefixes, and a trie takes advantage of that.  The downside of common trie implementations for instance with just the english language, is that every node has an array of size 26, and most of them go unused and are just wasted space. Since we are dealing with all ASCII combinations, not just the english language, then I would have to have an array of 256 on every node. That's alot of wasted space, and would make the trie much larger than it has to be.</br></br>In my implentation, I used a hashtable instead of an array.  The hashtable's key is a byte, which would correspond to the ASCII code. Since we're only dealing with 256, a byte can handle that.  The value is a pointer to the next trie node.  There is a count associated with every node, which represents how many times a word ended at that particular node. When you output the words to an output file, we need to traverse the trie. At every node we visit, we have to first sort the hashtable keys, and then visit the nodes in sorted order. As you traverse, you have to build the word by appending the char associated with that node. Whenever we reach a node where the count is greater than 0, we know that a word ended there, so we write it to the file along with its count. The other option was to store the word at the terminal node, but that would defeat purpose, because we would then be using as much memory as a hash table, and even more because of all the extra character nodes.
</br></br>We lose some performance here compared to the hashtable implementation, since the insertion time for the hashtable is O(1), but for the trie it is O(k) where k is the length of the word. But it should be way more efficient when it comes to space.
I didn't have time to figure out exactly how to measure the memory used for a certain object.
</br></br>Another concern is that the output words function for the trie is recursive, which could result in a stack overflow. An iterative function should be possible, but I didn't have time to work on it.</br></br>

* **Ternary Tree**
</br>A ternary search tree has the advantage of a trie, where prefixes of words can share space, btu doesn't have wasted space problem of the trie. The ternary tree implementaion is similar to the trie where each ternary node contains a byte which represents the character it is associated with, and the count of words that end at that node. The ternary is like a binary tree, where if a character is smaller than the current node, then we have to look for the node on the current nodes left branch. If it is greater, then we look for it on the current node's right branch.  If it is equal, however, then we found the node we were looking for, and now we move to the middle node to search for the next character of the word.
</br></br>The insertion time for a ternary tree is O(logk) where k is the length of the word.  Outputing the words to file is simply traversing the tree and building the word, just like the trie, and outputing it where the count is greater than 0.  We have the added benefit that the tree is already sorted, so there is no extra sorting needed here.  The function is recursive, so I have the same concern as before. 
</br></br>This data structure, according to articles I've read, is supposed to be much more memory efficient than a hash table, and could theoretically run just as fast.  However, so far in my testing, the hashtabe ran much faster. And like I mentioned before, I haven't worked out how to measure the memory yet.
</br></br>
*  **DAWG - Directed Acyclic Word Graph**  &nbsp;&nbsp;&nbsp; *(Honorable Mention)* 
</br>I read about this data structure, and how memory efficient it's supposed to be at storing a large number of words. It is like a trie where it uses prefixes of words to save space, but it also uses suffixes as well.  I'm not sure exactly how it's done, and if it's at all possible to store how many occurrences you have for each word, since multiple words terminate at the same node.
It seemed like a complicated structure to implement, and I didn't have time to explore it further.

</br>
###Median algorithm

In order to calculate the median, we would have to keep a history of all "unique words in a line" elements and keep them in some sort of data structure like an array. Then maybe use the min heap/max heap approach to figure out the median very fast, but we would still have to store a huge number of elements. This wouldn't scale very well. 

This is the solution I came up with.
I took advantage of the fact that the maximum length of a tweet is 140 characters.  The shortest word is 1 character long.
So the maximum of words we could have in 1 tweet is 70.  For instance, "a b c d ", and so on.
I created an array whose size is half the length of the maximum tweet length.  So currently it would be an array of 70.
But this is configurable.  In app.config, there is a setting, "maxTweetLength", which can be modified, if Twitter should ever decide to increase the limit.

Here is how the array is used. Each array index represents a number of unique words. For instance, index 0 represents 1 unique word in a line. index 69 represents 70 unique words in a line.  The values are the number of lines that we've read that have the number of unique words equal to the index + 1. So for instance medianArray[12] = 30 means that there are 30 lines that we've read with 13 unique words in it. 

To calculate the median, lets use an example.  Lets say our array looks like:
medianArray[6] = 5
medianArray[12] = 3
medianArray[16] = 8
The rest of the values are 0.

This means that if we had stored each line's number of unique words in memory, the array would like this
{7,7,7,7,7,13,13,13,17,17,17,17,17,17,17,17}
Since the length of this array is even, then we take the 2 middle elements, 13 and 17, and return the average, 15.

This process takes constant time, since our array is fixed at size 70, and we're just doing 1 pass on it every time we
want to calculate the media.

When we read the file, everytime we encounter a new line, we perform this median operation, and output the new median to the output file.

When we finish inserting all of the words to the data structure, the median process is done, and this is where we begin to output all the words.

</br>
###Testing

Machine: MacBook Pro OS X 2.26 GHz Intel Core 2 Duo with 4GB 1333 MHz DDR3

| File Size | Lines | Words Per Line | Data Structure | Time | Memory Used (bytes) |
|:---------:|:----------:|:--------------:|----------------|:------------:|:-------------------:|
| 128MB | 1,000,000 | 12 | Hashtable | 24.9739763 | 16,784,608 |
| 128MB | 1,000,000 | 12 | Trie | 52.8724920 | 188,125,224 |
| 128MB | 1,000,000 | 12 | Ternary | 23.9138333 | 31,765,928 |
| 1.28GB | 10,000,000 | 12 | Hashtable | 3:15.5897176 | 16,795,832 |
| 1.28GB | 10,000,000 | 12 | Trie | 8:48.4196140 | 188,285,328 |
| 1.28GB | 10,000,000 | 12 | Ternary | 4:05.9391220 | 31,781,544 |

*Memory is calculated as amount of memory used after data structure has been filled minus memory used before reading the file*