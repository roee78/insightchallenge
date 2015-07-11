#!/bin/sh

function usage
{
	echo
	echo
    	echo "You may choose a data structure to store the words. If you don't choose one, it will default to hash table"

	echo
	echo "Example"
	echo
        echo "run.sh -ds 0   =   hash table"
        echo "run.sh -ds 1   =   trie"
        echo "run.sh -ds 2   =   ternary tree"
	echo
	echo
}

while [ "$1" != "" ]; do
    case $1 in
        -f | --file )           shift
                                filename=$1
                                ;;
	-ds | --datastructure ) shift
                                dataStructure=$1
                                ;;
        -h | --help )           usage
                                exit
                                ;;
        * )                     usage
                                exit 1
    esac
    shift
done

mono "./TweetStats/TweetStats/bin/Release/TweetStats.exe" $dataStructure