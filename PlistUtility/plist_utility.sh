#!/bin/bash

# c0lby:

#####################################################
##############  DO NOT EDIT THIS FILE  ##############
#####################################################

projectDir=
release=
copyright=

while getopts "p:r:c:" object; do
	case "${object}" in
		p) projectDir="${OPTARG}" ;;
		r) release="${OPTARG}" ;;
		c) copyright="${OPTARG}" ;;
	esac
done
shift $((OPTIND-1))


#### Get Values from Info.plist ####


infoPlist="$projectDir/Info.plist"
rootPlist=

projTypeMac="Mac"
projTypeiOS="iOS"

echo $infoPlist

# Check for a couple common Keys that would be present in most iOS Info.plist files
projType=$(/usr/libexec/PlistBuddy -c "Print :LSRequiresIPhoneOS" $infoPlist 2>/dev/null \
	|| /usr/libexec/PlistBuddy -c "Print :UIMainStoryboardFile" $infoPlist 2>/dev/null \
	|| printf $projTypeMac) # if they're not found, assume it's Mac


# Set up some platform specific stuff
if [ "$projType" = "$projTypeMac" ]; then
	echo Project Type: OSX
	# Mac copyright text should be in the Info.plist file, so check and default to the c arg if not
	copyright=$(/usr/libexec/PlistBuddy -c "Print :NSHumanReadableCopyright" $infoPlist 2>/dev/null || printf "$copyright")
else
	echo Project Type: iOS
	# If projType doesn't equal Mac, it's iOS
	projType="$projTypeiOS"
	rootPlist="$projectDir/Settings.bundle/Root.plist"
fi

echo $copyright

# Get the BundleShortVersionString (Version Number) from from Info.plist and set to a variable
versionNumber=$(/usr/libexec/PlistBuddy -c "Print :CFBundleShortVersionString" $infoPlist)

# Get the BundleVersion (Build Number) from from Info.plist and set to a variable
buildNumber=$(/usr/libexec/PlistBuddy -c "Print :CFBundleVersion" $infoPlist)

# Get the SVN Version (Revision) of the Solution and set to a variable
svnVersion=$(/usr/bin/svnversion "${SolutionDir}")

# Get the Git Commit (Short Hash) of the Solution and set to a variable
gitCommitHash=$(/usr/bin/git rev-parse --short HEAD)


IncrementBuildNumber ()
{
	buildNumber=$((++buildNumber))
	/usr/libexec/PlistBuddy -c "Set :CFBundleVersion $buildNumber" $infoPlist
}

IncrementVersionNumber ()
{
	a=( ${versionNumber//./ } )                   # replace points, split into array
	((a[2]++))                                    # increment revision (or other part)
	versionNumber="${a[0]}.${a[1]}.${a[2]}"       # compose new version
	/usr/libexec/PlistBuddy -c "Set :CFBundleShortVersionString $versionNumber" $infoPlist
}

if [ "$release" = true ]; then
	IncrementBuildNumber
	IncrementVersionNumber
fi

# Create Some variables to create hold our Preference node values
nodeKey=
nodeEntry=
nodeTitle=
nodeValue=
nodeDefault=


# Set up a counter to increment the index of the PreferenceSpecifiers
nodeIndex=-1

PreparePreferenceFile ()
{
	/usr/libexec/PlistBuddy -c "Clear dict" $rootPlist
	/usr/libexec/PlistBuddy -c "Add :PreferenceSpecifiers array" $rootPlist
}


AddNewDictionaryNode ()
{
	/usr/libexec/PlistBuddy -c "Add :PreferenceSpecifiers:$((++nodeIndex)) dict" $rootPlist
}


AddNewStringNode ()
{
	local OPTIND

	while getopts "e:v:" o; do
		case "${o}" in
			e) nodeEntry="${OPTARG}" ;;
			v) nodeValue="${OPTARG}" ;;
		esac
	done

	shift $((OPTIND-1))

	/usr/libexec/PlistBuddy -c "Add :PreferenceSpecifiers:$nodeIndex:$nodeEntry string $nodeValue" $rootPlist
}


AddNewBoolNode ()
{
	local OPTIND

	while getopts "e:v:" o; do
		case "${o}" in
			e) nodeEntry="${OPTARG}" ;;
			v) nodeValue="${OPTARG}" ;;
		esac
	done

	shift $((OPTIND-1))

	/usr/libexec/PlistBuddy -c "Add :PreferenceSpecifiers:$nodeIndex:$nodeEntry bool $nodeValue" $rootPlist
}


AddNewArrayNode ()
{
	local OPTIND

	while getopts "e:" o; do
		case "${o}" in
			e) nodeEntry="${OPTARG}" ;;
		esac
	done

	shift $((OPTIND-1))

	/usr/libexec/PlistBuddy -c "Add :PreferenceSpecifiers:$nodeIndex:$nodeEntry array" $rootPlist
}


AddNewPreferenceGroup()
{
	local OPTIND

	while getopts "t:" o; do
		case "${o}" in
			t) nodeTitle="${OPTARG}" ;;
		esac
	done

	shift $((OPTIND-1))

	AddNewDictionaryNode
	AddNewStringNode -e Type 	-v PSGroupSpecifier
	AddNewStringNode -e Title  	-v "${nodeTitle}"
}


AddNewTitleValuePreference ()
{
	local OPTIND

	while getopts "k:d:t:" o; do
		case "${o}" in
			k) nodeKey="${OPTARG}" ;;
			d) nodeDefault="${OPTARG}" ;;
			t) nodeTitle="${OPTARG}" ;;
		esac
	done

	shift $((OPTIND-1))

	AddNewDictionaryNode
	AddNewStringNode -e Type 	-v PSTitleValueSpecifier

	AddNewStringNode -e Key  			-v "${nodeKey}"
	AddNewStringNode -e DefaultValue  	-v "${nodeDefault}"
	AddNewStringNode -e Title  			-v "${nodeTitle}"
}


AddNewTextFieldPreference ()
{
	local OPTIND

	while getopts "k:d:t:" o; do
		case "${o}" in
			k) nodeKey="${OPTARG}" ;;
			d) nodeDefault="${OPTARG}" ;;
			t) nodeTitle="${OPTARG}" ;;
		esac
	done

	shift $((OPTIND-1))

	AddNewDictionaryNode
	AddNewStringNode -e Type 	-v PSTextFieldSpecifier

	AddNewStringNode -e Key  			-v "${nodeKey}"
	AddNewStringNode -e DefaultValue  	-v "${nodeDefault}"
	AddNewStringNode -e Title  			-v "${nodeTitle}"
	AddNewStringNode -e KeyboardType	-v URL
}


AddNewMultiValuePreference ()
{
	local OPTIND

	while getopts "k:d:t:" o; do
		case "${o}" in
			k) nodeKey="${OPTARG}" ;;
			d) nodeDefault="${OPTARG}" ;;
			t) nodeTitle="${OPTARG}" ;;
		esac
	done

	shift $((OPTIND-1))

	AddNewDictionaryNode
	AddNewStringNode -e Type 	-v PSMultiValueSpecifier

	AddNewStringNode -e Key  			-v "${nodeKey}"
	AddNewStringNode -e DefaultValue  	-v "${nodeDefault}"
	AddNewStringNode -e Title  			-v "${nodeTitle}"
}


SetMultiValuePreferenceTitles ()
{
	AddNewArrayNode  -e Titles

	counter=0

	for var; do
		AddNewStringNode  -e Titles:$counter  -v "${var}"
		counter=$((counter+1))
	done
}


SetMultiValuePreferenceValues ()
{
	AddNewArrayNode  -e Values

	counter=0

	for var; do
		AddNewStringNode  -e Values:$counter  -v "${var}"
		counter=$((counter+1))
	done

}

AddNewToggleSwitchPreference ()
{
	local OPTIND

	while getopts "k:d:t:" o; do
		case "${o}" in
			k) nodeKey="${OPTARG}" ;;
			d) nodeDefault="${OPTARG}" ;;
			t) nodeTitle="${OPTARG}" ;;
		esac
	done

	shift $((OPTIND-1))

	AddNewDictionaryNode
	AddNewStringNode -e Type 	-v PSToggleSwitchSpecifier

	AddNewStringNode -e Key  			-v "${nodeKey}"
	AddNewBoolNode	 -e DefaultValue  	-v "${nodeDefault}"
	AddNewStringNode -e Title  			-v "${nodeTitle}"
}