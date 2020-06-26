#REMEMBER TO SETUP PIPELINE VARIABLE
if [ ! -n $(DpsIdScope) ]
then
    echo "You need define the DpsIdScope variable in VSTS"
    exit
fi

#PATH TO CONSTANTS FILE
APP_CONSTANT_FILE=XamTrack.Core/appconfig.json

if [ -e "$APP_CONSTANT_FILE" ]
then
    echo "Updating App Secret Values to DpsIdScope in appconfig.json"
    sed -i '' 's#"DpsIdScope": "[a-z:./\\_]*"#"DpsIdScope": "$(DpsIdScope)"#' $APP_CONSTANT_FILE

    echo "Updating App Secret Values to IotHubConnectionString in appconfig.json"
    sed -i '' 's#"IotHubConnectionString": "[a-z:./\\_]*"#"IotHubConnectionString": "$(IotHubConnectionString)"#' $APP_CONSTANT_FILE

    echo "Updating App Secret Values to DpsSymetricKey in appconfig.json"
    sed -i '' 's#"DpsSymetricKey": "[a-z:./\\_]*"#"DpsSymetricKey": "$(DpsSymetricKey)"#' $APP_CONSTANT_FILE

    echo "Updating App Secret Values to SharedKey in appconfig.json"
    sed -i '' 's#"SharedKey": "[a-z:./\\_]*"#"SharedKey": "$(SharedKey)"#' $APP_CONSTANT_FILE

    echo "File content:"
    cat $APP_CONSTANT_FILE
fi