//The following are the three Lambda functions used in Pokebones, pasted for easy viewing.

//Add User: https://09eq856ygl.execute-api.us-east-1.amazonaws.com/default/AddUser
//import json
//import boto3

//def lambda_handler(event, context):
//    dynamodb = boto3.resource('dynamodb')
//    table = dynamodb.Table('Assign4Players')

//    if 'body' in event and event['body']:
//        request_text = event['body']
//body = json.loads(request_text)
//        if 'password' in body and 'username' in body:
//            if not body ['username']:
//                return error_object('Bad request - username cannot be blank')
//            if not body ['password']:
//                return error_object('Bad request - password cannot be blank')
//            user_id = body ['username']
//try_pwd = body ['password']

//check_user = table.get_item(Key={ 'user_id':user_id})
//            if 'Item' in check_user and check_user ['Item']:
//                return error_object('Error - user with username '+ body ['username'] + ' already exists. Try another!')

//            new_entry = {
//    'user_id' : body['username'],
//                'password': body['password'],
//                'playerSkill': '0'
//            }
//table.put_item(
//                Item = new_entry
//            )
//            return {
//    'statusCode': 200,
//                'body': 'New user added?'
//            }
//        else:
//            return error_object('must send username and pawwsord')
//    else:
//        return error_object('Must have body on your request')        
//def error_object(error_message):
//    return {
//    'statusCode': 200,
//        'body': '{"error":"' + error_message + '"}'
//    }

//Sign in: https://wxrtjphwqg.execute-api.us-east-1.amazonaws.com/default/Assign4func
//import json
//import boto3

//def lambda_handler(event, context):
//    dynamodb = boto3.resource('dynamodb')
//    table = dynamodb.Table('Assign4Players')

//    if 'body' in event and event['body']:
//        request_text = event['body']
//body = json.loads(request_text)
//        if 'password' in body and 'username' in body:
//            if not body ['username']:
//                return error_object('Bad request - username cannot be blank')
//            if not body ['password']:
//                return error_object('Bad request - password cannot be blank')
//            user_id = body ['username']
//try_pwd = body ['password']
//response = table.get_item(
//                Key={ 'user_id': user_id }
//            )
//            item = response ['Item']
//            if try_pwd == item ['password']:
//                return {
//    'statusCode': 200,
//                    'body': json.dumps(item)
//                }
//            else:
//                return error_object('Password does not match.')
//        else:
//            return error_object('must send username and pawwsord')
//    else:
//        return error_object('Must have body on your request')        
//def error_object(error_message):
//    return {
//    'statusCode': 200,
//        'body': '{"error":"' + error_message + '"}'
//    }

//Update score: https://h5yn1jjwp6.execute-api.us-east-1.amazonaws.com/default/PokeBonesScoreUpdate
//import json
//import boto3

//def lambda_handler(event, context):
//    dynamodb = boto3.resource('dynamodb')
//    table = dynamodb.Table('Assign4Players')

//    if 'body' in event and event['body']:
//        request_text = event['body']
//body = json.loads(request_text)
//        if 'username' in body and 'score' in body:
//            if not body ['username']:
//                return error_object('Bad request - username cannot be blank')
//            if not body ['score']:
//                return error_object('Bad request - score cannot be blank')
//            user_id = body ['username']
//points = float(body ['score'])
//            player = table.get_item(
//                Key={ 'user_id':user_id}
//                )
//            item = player ['Item']
//real_score = float(item ['playerSkill'])
//            new_score =  real_score + points
//            db_score = str(new_score)
//            table.update_item(
//                Key ={
//    'user_id':user_id
//                },
//                UpdateExpression = 'SET playerSkill = :playerSkill',
//                ExpressionAttributeValues={
//    ':playerSkill': db_score,
//                }
//                )

//            return {
//    'statusCode': 200,
//                'body': json.dumps(item)
//            }
//        else:
//            return error_object('must send username and score')
//    else:
//        return error_object('Must have body on your request')        
//def error_object(error_message):
//    return {
//    'statusCode': 200,
//        'body': '{"error":"' + error_message + '"}'
//    }