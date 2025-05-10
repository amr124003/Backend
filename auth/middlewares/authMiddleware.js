const jwt = require('jsonwebtoken');

const verifyToken = (req, res, next) => {
  const token = req.headers['x-access-token'];

  if (!token) {
    return res.status(403).send({ message: 'No token provided!' });
  }

  jwt.verify(token, process.env.JWT_SECRET, (err, decoded) => {
    if (err) {
      return res.status(401).send({ message: 'Unauthorized!' });
    }
    req.userId = decoded.id;
    next();
  });
};

const protectRoute = (req, res, next) => {
  // This is a placeholder.
  // In a real application, you would check if the user has the necessary permissions
  // based on their role or other criteria stored in the token or retrieved from the database.
  console.log("Protecting route...");
  next();
};

module.exports = {
  verifyToken,
  protectRoute
};