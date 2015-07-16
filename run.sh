#!/bin/sh

function usage
{
    echo
    echo
    echo "Usage is: run.sh [options]"
    echo
    echo "options:"
    echo "     -c              Compiles the solution"  
    echo "     -ds [0,1,2]     Sets the data structure"  
    echo "                        0 = hash table"
    echo "                        1 = trie"
    echo "                        2 = ternary tree"
    echo "     -h              Displays usage information"  
	echo
	echo
}

function compile
{
    xbuild /p:Configuration=Release src/TweetStats/TweetStats.sln
}

while [ "$1" != "" ]; do
    case $1 in
        -f | --file )           shift
                                filename=$1
                                ;;
	-c | —-compile )        compile
				exit
				;;
	-ds | --dataStructure ) shift
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

mono "./src/TweetStats/TweetStats/bin/Release/TweetStats.exe" $dataStructure