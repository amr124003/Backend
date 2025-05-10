const User = require('../models/User');

const createUser = async (userData) => {
  try {
    const user = new User(userData);
    await user.save();
    return user;
  } catch (error){
    console.error('Error creating user:', error);
    throw error;
  }
};

const findUserByUsernameOrEmail = async (usernameOrEmail) => {
  try {
    const user = await User.findOne({
      $or: [{ username: usernameOrEmail }, { email: usernameOrEmail }]
    });
    return user;
  } catch (error) {
    console.error('Error finding user:', error);
    throw error;
  }
};

module.exports = {
  createUser,
  findUserByUsernameOrEmail,
};